// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AddBusinessDays.cs
//  Copyright:  Engage Inc. 2010
//              www.engage2day.com
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity adds business days to a given date or 
//              determines the next business day after X normal days.
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using ManipulationLibrary.Dates.Helpers;

namespace ManipulationLibrary.Dates
{
    public sealed class AddBusinessDays : CodeActivity
    {

        protected override void Execute(CodeActivityContext executionContext)
        {
            var rules = DateUtilities.GetRules(executionContext);
            
            //Change the result based on the rules such as Check OnlyLastDay, etc.
            var result = DateUtilities.ModifyDateTime(rules,
                CheckOnlyLastDay.Get<bool>(executionContext), 
                StartDate.Get<DateTime>(executionContext), 
                Operations.Add, 
                DaysToAdd.Get<int>(executionContext),
                HoursToAdd.Get<int>(executionContext),
                MinutesToAdd.Get<int>(executionContext));

            Result.Set(executionContext, result);
        }
        
        [Output("Result")]
        [Default("1900-01-01T00:00:00Z")]
        public OutArgument<DateTime> Result { get; set; }

        [Input("Start Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("Only check to make sure the last day is a business day")]
        [Default("false")]
        public InArgument<bool> CheckOnlyLastDay { get; set; }

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

