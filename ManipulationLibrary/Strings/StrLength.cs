// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		StrLength.cs
//  Summary:    Get the length of a string.
// ==================================================================================
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    [WorkflowActivity("Length", "String Utilities")]
    public sealed class StrLength : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            Length.Set(executionContext, Text.Get<string>(executionContext).Length);
        }


        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Length")]
        public OutArgument<int> Length { get; set; }

    }
}
