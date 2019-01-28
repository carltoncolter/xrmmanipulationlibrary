// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		Capitalize.cs
//  Summary:    Capitalize the beginning of words in a string
// ==================================================================================
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Capitalize : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var text = Text.Get<string>(executionContext);
            string result;

            if (CapAll.Get<bool>(executionContext))
            {
                // All words
                result = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
            } else
            {
                // First Letter only
                result = text.Substring(0, 1).ToUpper() + text.Substring(1);
            }

            Result.Set(executionContext,result);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        [Input("Capitalize All Words")]
        [Default("True")]
        public InArgument<bool> CapAll { get; set; }
    }
}
