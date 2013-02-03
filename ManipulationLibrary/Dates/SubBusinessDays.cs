// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		SubBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity Subtracts business days to a given date or 
//              determines the next business day after X normal days.
// ==================================================================================
using System;
using System.Activities;
using ManipulationLibrary.Helpers;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Dates
{
    public sealed class SubBusinessDays : CodeActivity
    {

        protected override void Execute(CodeActivityContext executionContext)
        {
            TimeSpan? begin = DateUtilities.ParseTimeSpan(BusinessTimeStart.Get<string>(executionContext));
            TimeSpan? end = DateUtilities.ParseTimeSpan(BusinessTimeEnd.Get<string>(executionContext));

            var rules = DateUtilities.GetRules(executionContext);
            
            //Change the result based on the rules such as Check OnlyLastDay, etc.
            var result = DateUtilities.ModifyDateTime(rules,
                CheckOnlyLastDay.Get<bool>(executionContext), 
                StartDate.Get<DateTime>(executionContext), 
                Operations.Subtract, 
                DaysToSubtract.Get<int>(executionContext),
                HoursToSubtract.Get<int>(executionContext),
                MinutesToSubtract.Get<int>(executionContext), begin, end);

            Result.Set(executionContext, result);
        }
        
        [Output("Result")]
        [Default("1900-01-01T00:00:00Z")]
        public OutArgument<DateTime> Result { get; set; }

        [Input("Start Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("Business Time Start")]
        [Default("08:00")]
        public InArgument<string> BusinessTimeStart { get; set; }

        [Input("Business Time End")]
        [Default("18:00")]
        public InArgument<string> BusinessTimeEnd { get; set; }

        [Input("Only check to make sure the last day is a business day")]
        [Default("false")]
        public InArgument<bool> CheckOnlyLastDay { get; set; }

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

