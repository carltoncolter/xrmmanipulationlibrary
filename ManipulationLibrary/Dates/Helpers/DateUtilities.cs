// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
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
// ==================================================================================

using System;
using System.Linq;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Query;
using Microsoft.Crm.SdkTypeProxy;

namespace ManipulationLibrary.Dates.Helpers
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
        ///   Change the date X days from the current date
        /// </summary>
        /// <param name = "date">The date to modify</param>
        /// <param name = "days">The number of days to change</param>
        /// <param name = "operation">The operation to add or subtract</param>
        public static DateTime ChangeXDays(DateTime date, int days, Operations operation)
        {
            var span = new TimeSpan(days, 0, 0, 0);
            return operation == Operations.Add ? date.Add(span) : date.Subtract(span);
        }

        /// <summary>
        ///   Check to see if a date is a holiday using the calendarrules.
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "date">The date to check</param>
        /// <returns>true for holiday, false for not a holiday</returns>
        public static Boolean CheckHoliday(calendarrule[] calRules, DateTime date)
        {
            if (calRules.Length == 0)
            {
                return false;
            }

            return (from rule in calRules
                    let start = DateTime.Parse(rule.effectiveintervalstart.Value)
                    let end = DateTime.Parse(rule.effectiveintervalend.Value)
                    where date > start && date < end
                    select start).Any();
        }

        /// <summary>
        ///   Find the date X business days from the start date.
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "startDate">The start date</param>
        /// <param name = "daysToAdd">The number of business days to add</param>
        /// <param name = "operation">The operation to add or subtract</param>
        /// <returns>The start date plus the number of business days specified</returns>
        public static DateTime FindDateXBizDays(calendarrule[] calRules, DateTime startDate, int daysToAdd,
                                                Operations operation)
        {
            var day = new TimeSpan(1, 0, 0, 0);

            var oDate = new DateTime();
            if (startDate != DateTime.MinValue)
            {
                oDate = startDate;
            }

            while (daysToAdd > 0)
            {
                //Check to see if we're doing an add or remove days operation, determine what the next day will be either way
                oDate = operation == Operations.Add ? oDate.Add(day) : oDate.Subtract(day);

                //If it is a business day, decrease the counter of the days left to add or subtract
                if (IsBusinessDay(calRules, oDate))
                {
                    daysToAdd--;
                }
            }

            return oDate;
        }

        /// <summary>
        ///   Add or subtract days from a given date
        /// </summary>
        /// <param name = "startDate">The start date</param>
        /// <param name = "daysToAdd">The number of days to add</param>
        /// <param name = "operation">Whether to subtract or add</param>
        /// <returns></returns>
        public static DateTime FindDateXDays(DateTime startDate, int daysToAdd, Operations operation)
        {
            if (daysToAdd != 0)
            {
                switch (operation)
                {
                    case Operations.Add:
                        return startDate.AddDays(daysToAdd);
                    case Operations.Subtract:
                        return startDate.Subtract(new TimeSpan(daysToAdd, 0, 0, 0));
                }
            }

            return startDate;
        }

        /// <summary>
        ///   Get the Calendar Rules
        /// </summary>
        /// <param name = "org">The CRM Organization</param>
        /// <param name = "crmService">The CRM Service</param>
        /// <returns>An array of the calendar rules</returns>
        public static calendarrule[] GetCal(organization org, CrmService crmService)
        {
            var bizCal =
                (calendar)
                crmService.Retrieve(EntityName.calendar.ToString(), org.businessclosurecalendarid.Value,
                                    new AllColumns());
            return bizCal.calendarrules;
        }

        /// <summary>
        ///   Get the Calendar Rules
        /// </summary>
        /// <param name = "org">The CRM Organization</param>
        /// <param name = "crmService">The CRM Service Interface</param>
        /// <returns>An array of the calendar rules</returns>
        public static calendarrule[] GetCal(organization org, ICrmService crmService)
        {
            var bizCal =
                (calendar)
                crmService.Retrieve(EntityName.calendar.ToString(), org.businessclosurecalendarid.Value,
                                    new AllColumns());
            return bizCal.calendarrules;
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
        public static DateTime ModifyDateTime(calendarrule[] calRules, bool checkLastDayOnly, DateTime date,
                                              Operations operation, int days, int hours, int minutes)
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
                        date = ChangeXDays(date, days - 1, operation);
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
            if (date.Hour > 18)
            {
                adjustend = new DateTime(adjustend.Year, adjustend.Month, adjustend.Day,
                                         operation == Operations.Add ? 8 : 18, 0, 0);
                date = FindDateXBizDays(calRules, adjustend, 1, operation);
            }

            else if (date.Hour < 8)
            {
                adjustend = new DateTime(adjustend.Year, adjustend.Month, adjustend.Day,
                                         operation == Operations.Add ? 8 : 18, 0, 0);
                date = adjustend;
            }

            return date;
        }

        /// <summary>
        ///   Determins if a day is a business day
        /// </summary>
        /// <param name = "calRules">The list of calendarrules from the Business Closure calendar</param>
        /// <param name = "date">The date to check</param>
        /// <returns>true if is a business day, false if it is not a business day</returns>
        private static bool IsBusinessDay(calendarrule[] calRules, DateTime date)
        {
            // if it is Sat, Sun, or Holiday, then it is not a business day
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return !CheckHoliday(calRules, date);
            }
        }
    }
}