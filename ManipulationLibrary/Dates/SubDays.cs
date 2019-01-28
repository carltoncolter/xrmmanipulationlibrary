// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		SubDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity Subtracts a given number of days to another date.
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Dates
{
    public sealed class SubDays : CodeActivity
    {

        protected override void Execute(CodeActivityContext executionContext)
        {
            var result = new DateTime();
            var start = StartDate.Get<DateTime>(executionContext);
                        
            if (start != DateTime.MinValue)
                result = start;

            if (start != DateTime.MinValue && DaysToSubtract.Get<int>(executionContext) != 0)
            {
                result = start.AddYears(-1*YearsToSubtract.Get<int>(executionContext))
                              .AddMonths(-1*MonthsToSubtract.Get<int>(executionContext))
                              .AddDays((-7*WeeksToSubtract.Get<int>(executionContext))
                              +        (-1*DaysToSubtract.Get<int>(executionContext)))
                              .AddHours(-1*HoursToSubtract.Get<int>(executionContext))
                              .AddMinutes(-1*MinutesToSubtract.Get<int>(executionContext));
            }

            Result.Set(executionContext, result);
        }

        [Output("Result")]
        [Default("1900-01-01T00:00:00Z")]
        public OutArgument<DateTime> Result { get; set; }

        [Input("Start Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("Years To Subtract")]
        [Default("0")]
        public InArgument<int> YearsToSubtract { get; set; }

        [Input("Months To Subtract")]
        [Default("0")]
        public InArgument<int> MonthsToSubtract { get; set; }

        [Input("Weeks To Subtract")]
        [Default("0")]
        public InArgument<int> WeeksToSubtract { get; set; }

        [Input("Days To Subtract")]
        [Default("0")]
        public InArgument<int> DaysToSubtract { get; set; }

        [Input("Hours To Subtract")]
        [Default("0")]
        public InArgument<int> HoursToSubtract { get; set; }

        [Input("Minutes To Subtract")]
        [Default("0")]
        public InArgument<int> MinutesToSubtract { get; set; }
    }
}

