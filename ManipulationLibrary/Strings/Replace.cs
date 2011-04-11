// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Replace.cs
// ==================================================================================
using System;
using System.Activities;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Replace : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var text = Text.Get<string>(executionContext);
            var old = Old.Get<string>(executionContext);
            
            var @new = New.Get<string>(executionContext);
            if (@new == null) @new = String.Empty;

            var result = string.Empty;
            if (!CaseSensitive.Get<bool>(executionContext))
            {
                if (!String.IsNullOrEmpty(text) && !String.IsNullOrEmpty(old))
                {
                    result = text.Replace(old, @new);
                }
            } else
            {
                result = CompareAndReplace(text, old, @new, StringComparison.CurrentCultureIgnoreCase);
            }
            Result.Set(executionContext, result);
        }

        private static string CompareAndReplace (string text, string old, string @new, StringComparison comparison)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(old)) return text;

            var result = new StringBuilder();
            var oldLength = old.Length;
            var pos = 0;
            var next = text.IndexOf(old, comparison);

            while (next>0)
            {
                result.Append(text, pos, next - pos);
                result.Append(@new);
                pos = next + oldLength;
                next = text.IndexOf(old, pos, comparison);
            }

            result.Append(text, pos, text.Length - pos);
            return result.ToString();
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        [Input("Old Value")]
        public InArgument<string> Old { get; set; }

        [Input("New Value")]
        public InArgument<string> New { get; set; }

        [Input("Case Sensitive")]
        [Default("False")]
        public InArgument<bool> CaseSensitive { get; set; }
    }
}
