// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Match.cs
// ==================================================================================
using System;
using System.Text.RegularExpressions;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.RegEx
{
    public sealed class Replace : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var invalid = false;
            var pattern = Pattern.Get<string>(executionContext);
            var replacement = Replacement.Get<string>(executionContext);
            var text = Text.Get<string>(executionContext);
            var result = text;

            try
            {
                var regex = new Regex(pattern);
                result = regex.Replace(text, replacement);
            }
            catch (ArgumentException)
            {
                invalid = true;
                // Syntax error in the regular expression
            }

            InvalidRegularExpression.Set(executionContext, invalid);
            Result.Set(executionContext, result);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Input("Replacement")]
        public InArgument<string> Replacement { get; set; }

        [Input("Regular Expression Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Output("Invalid Regular Expression")]
        [Default("False")]
        public OutArgument<bool> InvalidRegularExpression { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
