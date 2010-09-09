// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		FormatRegex.cs
//  Summary:	This workflow activity formats a string using regular expression
// ==================================================================================
using System;
using System.Text.RegularExpressions;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.RegEx
{
    public sealed class FormatRegex : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var invalid = false;
            var match = false;
            var pattern = Pattern.Get<string>(executionContext);
            var text = Text.Get<string>(executionContext);
            var format = Format.Get<string>(executionContext);
            
            try
            {
                var regex = new Regex(pattern);
                match = regex.IsMatch(text);
                if (match)
                {
                    text = regex.Replace(text, format);
                }
            }
            catch (ArgumentException)
            {
                // Syntax error in the regular expression
                invalid = true;
            }

            InvalidRegularExpression.Set(executionContext, invalid);
            MatchFound.Set(executionContext, match);
            Result.Set(executionContext, text);
            
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Input("Regular Expression Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Input("Format")]
        [Default("(${area}) ${prefix}-${number}")]
        public InArgument<string> Format { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        [Output("Invalid Regular Expression")]
        [Default("False")]
        public OutArgument<bool> InvalidRegularExpression { get; set; }

        [Output("Match Found")]
        [Default("False")]
        public OutArgument<bool> MatchFound { get; set; }
    }
}
