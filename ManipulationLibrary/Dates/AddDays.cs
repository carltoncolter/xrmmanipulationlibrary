// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		AddDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity adds a given number of days to another date.
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Dates
{
    public sealed class AddDays : CodeActivity
    {

        protected override void Execute(CodeActivityContext executionContext)
        {
            var result = new DateTime();
            var start = StartDate.Get<DateTime>(executionContext);
                        
            if (start != DateTime.MinValue)
                result = start;

            if (start != DateTime.MinValue && DaysToAdd.Get<int>(executionContext) != 0)
            {
                result = start.AddYears(YearsToAdd.Get<int>(executionContext))
                              .AddMonths(MonthsToAdd.Get<int>(executionContext))
                              .AddDays((7*WeeksToAdd.Get<int>(executionContext)) + 
                                       DaysToAdd.Get<int>(executionContext))
                              .AddHours(HoursToAdd.Get<int>(executionContext))
                              .AddMinutes(MinutesToAdd.Get<int>(executionContext));
            }

            Result.Set(executionContext, result);
        }

        [Output("Result")]
        [Default("1900-01-01T00:00:00Z")]
        public OutArgument<DateTime> Result { get; set; }

        [Input("Start Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("Years To Add")]
        [Default("0")]
        public InArgument<int> YearsToAdd { get; set; }

        [Input("Months To Add")]
        [Default("0")]
        public InArgument<int> MonthsToAdd { get; set; }

        [Input("Weeks To Add")]
        [Default("0")]
        public InArgument<int> WeeksToAdd { get; set; }

        [Input("Days To Add")]
        [Default("0")]
        public InArgument<int> DaysToAdd { get; set; }

        [Input("Hours To Add")]
        [Default("0")]
        public InArgument<int> HoursToAdd { get; set; }

        [Input("Minutes To Add")]
        [Default("0")]
        public InArgument<int> MinutesToAdd { get; set; }
    }
}

