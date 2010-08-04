// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AddBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity subtracts a given number of days from a given date
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Workflow;
using Microsoft.Crm.Sdk;

namespace ManipulationLibrary.Dates
{
    [CrmWorkflowActivity("Subtract Days From Date", "Date Utilities")]
    public partial class SubDaysFromDate : SequenceActivity
    {

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var result = new CrmDateTime();
            var start = DateTime.Parse(StartDate.Value);

            if (start != DateTime.MinValue)
                result = StartDate;

            if (start != DateTime.MinValue && DaysToSub.Value != 0)
            {
                result.Value = start.Subtract(new TimeSpan(DaysToSub.Value, HoursToSub.Value, MinutesToSub.Value, 0)).ToString(); 
            }

            Result = result;
            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(CrmDateTime), typeof(SubDaysFromDate));
        [CrmOutput("Result")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime Result
        {
            get { return (CrmDateTime)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(CrmDateTime), typeof(SubDaysFromDate));
        [CrmInput("Start Date")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime StartDate
        {
            get { return (CrmDateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }


        public static DependencyProperty DaysToSubProperty = DependencyProperty.Register("DaysToSub", typeof(CrmNumber), typeof(SubDaysFromDate));
        [CrmInput("Days To Subtract")]
        [CrmDefault("0")]
        public CrmNumber DaysToSub
        {
            get { return (CrmNumber)GetValue(DaysToSubProperty); }
            set { SetValue(DaysToSubProperty, value); }
        }

        public static DependencyProperty HoursToSubProperty = DependencyProperty.Register("HoursToSub", typeof(CrmNumber), typeof(SubDaysFromDate));
        [CrmInput("Hours To Subtract")]
        [CrmDefault("0")]
        public CrmNumber HoursToSub
        {
            get { return (CrmNumber)GetValue(HoursToSubProperty); }
            set { SetValue(HoursToSubProperty, value); }
        }

        public static DependencyProperty MinutesToSubProperty = DependencyProperty.Register("MinutesToSub", typeof(CrmNumber), typeof(SubDaysFromDate));
        [CrmInput("Minutes To Subtract")]
        [CrmDefault("0")]
        public CrmNumber MinutesToSub
        {
            get { return (CrmNumber)GetValue(MinutesToSubProperty); }
            set { SetValue(MinutesToSubProperty, value); }
        }
    }
}
