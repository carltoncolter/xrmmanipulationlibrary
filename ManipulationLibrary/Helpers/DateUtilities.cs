// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		DateUtilities.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This classes contain a series of functions to assist in date 
//    manipulation.  It was written by Engage Inc. (www.engage2day.com) and 
//    contributed to aadd Date Utilities to the manipulation library.
//
//    A special thanks to Engage for contributing to the Manipulation Library.
//                                                                    -Carlton Colter
//
//    This helper was updated to work with CRM 2011 (5.0).  It can be copied and 
//    used in plugins and other CRM applications.
// ==================================================================================

using System;
using System.Activities;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Helpers
{
    public enum Operations
    {
        Add,
        Subtract
    }

    [Serializable]
    public class DateUtilities
    {
        /// <summary>
        ///   Find the date X business days from the start date.
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "startDate">The start date</param>
        /// <param name = "days">The number of business days</param>
        /// <param name = "operation">The operation to add or subtract</param>
        /// <returns>The start date plus the number of business days specified</returns>
        public static DateTime FindDateXBizDays(EntityCollection calRules, DateTime startDate, int days,
                                                Operations operation)
        {
            var day = new TimeSpan(1, 0, 0, 0);

            var oDate = new DateTime();
            if (startDate != DateTime.MinValue)
            {
                oDate = startDate;
            }

            while (days > 0)
            {
                //Check to see if we're doing an add or remove days operation, determine what the next day will be either way
                oDate = operation == Operations.Add ? oDate.Add(day) : oDate.Subtract(day);

                //If it is a business day, decrease the counter of the days left to add or subtract
                if (IsBusinessDay(calRules, oDate))
                {
                    days--;
                }
            }

            return oDate;
        }

        /// <summary>
        ///   Get the Calendar Rules
        /// </summary>
        /// <param name = "executionContext">The workflow execution context</param>
        /// <returns>An array of the calendar rules</returns>
        public static EntityCollection GetRules(CodeActivityContext executionContext)
        {
            // Create the context
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            //Create a query to retrieve calendarrules.
            return GetRules(service, context.OrganizationId);
        }

        /// <summary>
        ///   Get the Calendar Rules
        /// </summary>
        /// <param name="service">A Connected CRM Service</param>
        /// <param name="orgId">The Organization ID</param>
        /// <returns>An array of the calendar rules</returns>
        public static EntityCollection GetRules(IOrganizationService service, Guid orgId)
        {
            // Must be connected
            if (service == null) return null;
            
            var query = new QueryExpression
                        {
                            EntityName = "calendar",
                            ColumnSet = new ColumnSet {AllColumns = true},
                            LinkEntities =
                                {
                                    new LinkEntity
                                    {
                                        LinkFromEntityName = "calendar",
                                        LinkToEntityName = "organization",
                                        LinkFromAttributeName = "calendarid",
                                        LinkToAttributeName = "businessclosurecalendarid",
                                        LinkCriteria = new FilterExpression
                                                       {
                                                           FilterOperator = LogicalOperator.And,
                                                           Conditions =
                                                               {
                                                                   new ConditionExpression
                                                                   {
                                                                       AttributeName = "organizationid",
                                                                       Operator = ConditionOperator.Equal,
                                                                       Values = {orgId}
                                                                   }
                                                               }
                                                       }
                                    }
                                },
                            PageInfo = new PagingInfo
                                       {
                                           Count = 1,
                                           PageNumber = 1
                                       }
                        };

            var calendars = service.RetrieveMultiple(query).Entities;

            // If there was a calendar, return the rules (otherwise return null)
            return (calendars.Count == 0) ? null : (EntityCollection)calendars[0].Attributes["calendarrules"];
        }

        /// <summary>
        ///   Check to see if a date is a holiday using the calendarrules.
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "date">The date to check</param>
        /// <returns>true for holiday, false for not a holiday</returns>
        public static Boolean IsClosed(EntityCollection calRules, DateTime date)
        {
            if (calRules.Entities.Count == 0)
            {
                return false;
            }

            return (from rule in calRules.Entities
                    let start = (DateTime)rule.Attributes["effectiveintervalstart"]
                    let end = (DateTime)rule.Attributes["effectiveintervalend"]
                    where date >= start && date <= end
                    select start).Any();
        }

        /// <summary>
        ///   Modify the number of days according to the operation
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "checkLastDayOnly">Only verify the last day is a business day</param>
        /// <param name = "date">The date to modify</param>
        /// <param name = "operation">The operation to  be performed</param>
        /// <param name = "days">The number of days to modify by</param>
        /// <param name = "hours">The number of hours to modify by</param>
        /// <param name = "minutes">The number of minutes to modify by</param>
        public static DateTime ModifyDateTime(EntityCollection calRules, bool checkLastDayOnly, DateTime date,
                                              Operations operation, int days, int hours, int minutes, TimeSpan? mintime, TimeSpan? maxtime)
        {
            if (days != 0)
            {
                switch (checkLastDayOnly)
                {
                    case false:
                        date = FindDateXBizDays(calRules, date, days, operation);
                        break;
                    case true:
                        //When checkLastDayOnly is true, we only care if the last day is a business day. 
                        //So even if it's a thursday and we're adding four days, we'll return the next Monday, (since 4 days is a Sunday)
                        date = date.AddDays(((operation == Operations.Add) ? 1 : -1) * days - 1);
                        date = FindDateXBizDays(calRules, date, 1, operation);
                        break;
                }
            }

            //Once we have the date that's X business days from our initial date, we make adjustments for when the 
            // also wants to add hours and minutes
            var adjustend = date;
            adjustend = adjustend.AddHours(hours);
            adjustend = adjustend.AddMinutes(minutes);
            date = adjustend;

            //We check our adjusted end date to make sure it's within something close to normal business hours (8am - 6pm)
            //If not we go to the next day, or we round up to 8am
            if (maxtime.HasValue && mintime.HasValue)
            {
                if (date.TimeOfDay > maxtime)
                {
                    adjustend = new DateTime(adjustend.Year, adjustend.Month, adjustend.Day, 
                                             maxtime.Value.Hours, maxtime.Value.Minutes, maxtime.Value.Seconds);
                    date = FindDateXBizDays(calRules, adjustend, 1, operation);
                }

                else if (date.TimeOfDay < mintime)
                {
                    adjustend = new DateTime(adjustend.Year, adjustend.Month, adjustend.Day, 
                                             mintime.Value.Hours, mintime.Value.Minutes, mintime.Value.Seconds);
                    date = adjustend;
                }
            }

            return date;
        }

        /// <summary>
        ///   Determins if a day is a business day
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "date">The date to check</param>
        /// <returns>true if is a business day, false if it is not a business day</returns>
        private static bool IsBusinessDay(EntityCollection calRules, DateTime date)
        {
            // if it is Sat, Sun, or Holiday, then it is not a business day
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return !IsClosed(calRules, date);
            }
        }

        /// <summary>
        /// Convert string to a timespan
        /// </summary>
        /// <param name="time">Time in string format</param>
        /// <returns>Nullable TimeSpan </returns>
        public static TimeSpan? ParseTimeSpan(string time)
        {
            if (!time.Contains(":"))
            {
                return null;
            }

            try
            {
                var timeparts = time.Split(':');
                var intparts = new int[] { 0, 0, 0 };
                for (int i = 0; i < 3; i++)
                {
                    if (timeparts.Length > i)
                    {
                        intparts[i] = Convert.ToInt32(timeparts[i]);
                    }
                }
                return new TimeSpan(intparts[0], intparts[1], intparts[2]);
            } catch 
            {
                return null;
            }
        }
    }
}