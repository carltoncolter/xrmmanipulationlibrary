// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Trim.cs
// ==================================================================================
using System.Activities;

using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Trim : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            Result.Set(executionContext,Text.Get<string>(executionContext).Trim());
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
