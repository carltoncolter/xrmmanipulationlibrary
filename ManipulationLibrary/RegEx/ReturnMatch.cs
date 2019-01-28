// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		ReturnMatch.cs
// ==================================================================================
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.RegEx
{
    public sealed class ReturnMatch : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var invalid = false;
            var match = false;
            var pattern = Pattern.Get<string>(executionContext);
            var text = Text.Get<string>(executionContext);
            var index = Index.Get<int>(executionContext);
            var matchResult = string.Empty;
            try
            {
                var result = new StringBuilder();
                var regex = new Regex(pattern);

                var results = regex.Match(text);
                var pos = 1;

                match = results.Success;

                while (results.Success)
                {
                    if (pos==index)
                    {
                        result.Append(results.Value);
                        break;
                    } 
                    
                    if (pos==-1 && index>=1)
                    {
                        result.Append(results);
                    }
                    
                    results = results.NextMatch();
                    pos++;
                }

                if (result.Length>0)
                    matchResult = result.ToString();
            }
            catch (ArgumentException)
            {
                invalid = true;
                // Syntax error in the regular expression
            }

            InvalidRegularExpression.Set(executionContext,invalid);
            MatchFound.Set(executionContext, match);
            Match.Set(executionContext,matchResult);
            
        }

        [Input("Match Index (-1 for all, 1 for first)")]
        [Default("-1")]
        public InArgument<int> Index { get; set; }

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

        [Output("Match")]
        public OutArgument<string> Match { get; set; }

    }
}
