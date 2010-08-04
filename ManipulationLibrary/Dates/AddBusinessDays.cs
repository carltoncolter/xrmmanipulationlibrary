// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AddBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity adds business days to a given date or determines the next business day after X normal days.
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
    [CrmWorkflowActivity("Add Business Days to Date", "Date Utilities")]
    public partial class AddBusinessDays : SequenceActivity
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
            var rules =  DateUtilities.GetCal(org, crmService);
            
            var result = new CrmDateTime();
            
            //Change the result based on the rules such as Check OnlyLastDay, etc.
            result.Value = DateUtilities.ModifyDateTime(rules,
                                         CheckOnlyLastDay.Value,
                                         DateTime.Parse(StartDate.Value),
                                         Operations.Add,
                                         DaysToAdd.Value,
                                         HoursToAdd.Value,
                                         MinutesToAdd.Value).ToString();

            Result = result;

            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(CrmDateTime), typeof(AddBusinessDays));
        [CrmOutput("Result")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime Result
        {
            get { return (CrmDateTime)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(CrmDateTime), typeof(AddBusinessDays));
        [CrmInput("Start Date")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime StartDate
        {
            get { return (CrmDateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static DependencyProperty CheckOnlyLastDayProperty = DependencyProperty.Register("CheckOnlyLastDay", typeof(CrmBoolean), typeof(AddBusinessDays));
        [CrmInput("Make sure the last day is on a business day")]
        [CrmDefault("false")]
        public CrmBoolean CheckOnlyLastDay
        {
            get { return (CrmBoolean)GetValue(CheckOnlyLastDayProperty); }
            set { SetValue(CheckOnlyLastDayProperty, value); }
        }

        public static DependencyProperty DaysToAddProperty = DependencyProperty.Register("DaysToAdd", typeof(CrmNumber), typeof(AddBusinessDays));
        [CrmInput("Days To Add")]
        [CrmDefault("0")]
        public CrmNumber DaysToAdd
        {
            get { return (CrmNumber)GetValue(DaysToAddProperty); }
            set { SetValue(DaysToAddProperty, value); }
        }

        public static DependencyProperty HoursToAddProperty = DependencyProperty.Register("HoursToAdd", typeof(CrmNumber), typeof(AddBusinessDays));
        [CrmInput("Hours To Add")]
        [CrmDefault("0")]
        public CrmNumber HoursToAdd
        {
            get { return (CrmNumber)GetValue(HoursToAddProperty); }
            set { SetValue(HoursToAddProperty, value); }
        }

        public static DependencyProperty MinutesToAddProperty = DependencyProperty.Register("MinutesToAdd", typeof(CrmNumber), typeof(AddBusinessDays));
        [CrmInput("Minutes To Add")]
        [CrmDefault("0")]
        public CrmNumber MinutesToAdd
        {
            get { return (CrmNumber)GetValue(MinutesToAddProperty); }
            set { SetValue(MinutesToAddProperty, value); }
        }
    }
}

