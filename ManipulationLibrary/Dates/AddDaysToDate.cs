// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AddBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity adds a given number of days to another date.
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Workflow;
using Microsoft.Crm.Sdk;

namespace ManipulationLibrary.Dates
{
    [CrmWorkflowActivity("Add Days to Date", "Date Utilities")]
    public partial class AddDaysToDate : SequenceActivity
    {
        
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var result = new CrmDateTime();
            var start = DateTime.Parse(StartDate.Value);

            if (start != DateTime.MinValue)
                result = StartDate;

            if (start != DateTime.MinValue && DaysToAdd.Value != 0)
            {
                result.Value = start.AddDays(DaysToAdd.Value).AddHours(HoursToAdd.Value).AddMinutes(MinutesToAdd.Value).ToString();
            }
            
            Result = result;


            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(CrmDateTime), typeof(AddDaysToDate));
        [CrmOutput("Result")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime Result
        {
            get { return (CrmDateTime)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(CrmDateTime), typeof(AddDaysToDate));
        [CrmInput("Start Date")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime StartDate
        {
            get { return (CrmDateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }


        public static DependencyProperty DaysToAddProperty = DependencyProperty.Register("DaysToAdd", typeof(CrmNumber), typeof(AddDaysToDate));
        [CrmInput("Days To Add")]
        [CrmDefault("0")]
        public CrmNumber DaysToAdd
        {
            get { return (CrmNumber)GetValue(DaysToAddProperty); }
            set { SetValue(DaysToAddProperty, value); }
        }

        public static DependencyProperty HoursToAddProperty = DependencyProperty.Register("HoursToAdd", typeof(CrmNumber), typeof(AddDaysToDate));
        [CrmInput("Hours To Add")]
        [CrmDefault("0")]
        public CrmNumber HoursToAdd
        {
            get { return (CrmNumber)GetValue(HoursToAddProperty); }
            set { SetValue(HoursToAddProperty, value); }
        }

        public static DependencyProperty MinutesToAddProperty = DependencyProperty.Register("MinutesToAdd", typeof(CrmNumber), typeof(AddDaysToDate));
        [CrmInput("Minutes To Add")]
        [CrmDefault("0")]
        public CrmNumber MinutesToAdd
        {
            get { return (CrmNumber)GetValue(MinutesToAddProperty); }
            set { SetValue(MinutesToAddProperty, value); }
        }
    }
}

