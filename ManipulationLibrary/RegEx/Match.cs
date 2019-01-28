// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		Match.cs
// ==================================================================================
using System;
using System.Text.RegularExpressions;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.RegEx
{
    public sealed class Match : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var invalid = false;
            var match = false;
            var pattern = Pattern.Get<string>(executionContext);
            var text = Text.Get<string>(executionContext);

            try
            {
                var regex = new Regex(pattern);
                var result = regex.Match(text);
                match = result.Success;
            }
            catch (ArgumentException)
            {
                invalid = true;
                // Syntax error in the regular expression
            }

            InvalidRegularExpression.Set(executionContext, invalid);
            MatchFound.Set(executionContext, match);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Input("Regular Expression Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Output("Invalid Regular Expression")]
        [Default("False")]
        public OutArgument<bool> InvalidRegularExpression { get; set; }

        [Output("Match Found")]
        [Default("False")]
        public OutArgument<bool> MatchFound { get; set; }
    }
}
