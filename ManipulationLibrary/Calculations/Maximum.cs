// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Maximum.cs
//  Summary:	This workflow activity returns the highest number between two objects
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Calculations
{
    [WorkflowActivity("Maximum", "Calculation Utilities")]
    public sealed class Maximum : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            Result.Set(executionContext,
                       Math.Max(Number1.Get<int>(executionContext), Number2.Get<int>(executionContext)));
        }

        [Input("First number")]
        [Default("0")]
        public InArgument<int> Number1 { get; set; }

        [Input("Second number")]
        [Default("0")]
        public InArgument<int> Number2 { get; set; }

        [Output("Result")]
        public OutArgument<int> Result { get; set; }
    }
}