// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AddBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity subtracts business days from a given date or determines the next business day before X normal days.
// ==================================================================================


using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Workflow;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Query;
using Microsoft.Crm.SdkTypeProxy;
using ManipulationLibrary.Dates.Helpers;

namespace ManipulationLibrary.Dates
{
    [CrmWorkflowActivity("Subtract Business Days From Date", "Date Utilities")]
    public partial class SubBusinessDays : SequenceActivity
    {
         
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {

            // Get the context service.
            var contextService = (IContextService)executionContext.GetService(typeof(IContextService));
            var context = contextService.Context;

            // Use the context service to create an instance of CrmService.
            var crmService = context.CreateCrmService();

            //Retrieving our calendar objects for holiday calculations
            ColumnSetBase cols = new AllColumns();
            var org = (organization)crmService.Retrieve(EntityName.organization.ToString(), context.OrganizationId, cols);
            var rules = DateUtilities.GetCal(org, crmService);

            //Create some BizDayDateTime objects to hold our initial and return values for easy manipulation
            var result = new CrmDateTime();
            
            //Change the result based on the rules such as Check OnlyLastDay, etc.
            result.Value = DateUtilities.ModifyDateTime(rules,
                                         CheckOnlyLastDay.Value,
                                         DateTime.Parse(StartDate.Value),
                                         Operations.Add,
                                         DaysToSub.Value,
                                         HoursToSub.Value,
                                         MinutesToSub.Value).ToString();

            Result = result;

            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(CrmDateTime), typeof(SubBusinessDays));
        [CrmOutput("Result")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime Result
        {
            get { return (CrmDateTime)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(CrmDateTime), typeof(SubBusinessDays));
        [CrmInput("Start Date")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime StartDate
        {
            get { return (CrmDateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static DependencyProperty CheckOnlyLastDayProperty = DependencyProperty.Register("CheckOnlyLastDay", typeof(CrmBoolean), typeof(SubBusinessDays));
        [CrmInput("Make sure the last day is on a business day")]
        [CrmDefault("false")]
        public CrmBoolean CheckOnlyLastDay
        {
            get { return (CrmBoolean)GetValue(CheckOnlyLastDayProperty); }
            set { SetValue(CheckOnlyLastDayProperty, value); }
        }

        public static DependencyProperty DaysToSubProperty = DependencyProperty.Register("DaysToSub", typeof(CrmNumber), typeof(SubBusinessDays));
        [CrmInput("Days To Subtract")]
        [CrmDefault("0")]
        public CrmNumber DaysToSub
        {
            get { return (CrmNumber)GetValue(DaysToSubProperty); }
            set { SetValue(DaysToSubProperty, value); }
        }

        public static DependencyProperty HoursToSubProperty = DependencyProperty.Register("HoursToSub", typeof(CrmNumber), typeof(SubBusinessDays));
        [CrmInput("Hours To Subtract")]
        [CrmDefault("0")]
        public CrmNumber HoursToSub
        {
            get { return (CrmNumber)GetValue(HoursToSubProperty); }
            set { SetValue(HoursToSubProperty, value); }
        }

        public static DependencyProperty MinutesToSubProperty = DependencyProperty.Register("MinutesToSub", typeof(CrmNumber), typeof(SubBusinessDays));
        [CrmInput("Minutes To Subtract")]
        [CrmDefault("0")]
        public CrmNumber MinutesToSub
        {
            get { return (CrmNumber)GetValue(MinutesToSubProperty); }
            set { SetValue(MinutesToSubProperty, value); }
        }
    }
}

