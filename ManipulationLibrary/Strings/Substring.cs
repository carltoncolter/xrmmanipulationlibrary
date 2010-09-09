// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Substring.cs
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Substring : CodeActivity
    {
        
        protected override void Execute(CodeActivityContext executionContext)
        {
            var result = PerformSubstring(Text.Get<string>(executionContext), StartIndex.Get<int>(executionContext), Length.Get<int>(executionContext), LeftToRight.Get<bool>(executionContext));
            Result.Set(executionContext,result);
        }

        private static string PerformSubstring(string result, int start, int length, bool lefttoright)
        {
            if (length <= 0 || start < 0)
            {
                result = String.Empty;
            }
            else
            {
                if (!lefttoright)
                {
                    start = result.Length - length - start;
                }
                result = result.Substring(start, length);
            }
            return result;
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        [Input("From Left To Right")]
        [Default("True")]
        public InArgument<bool> LeftToRight { get; set; }

        [Input("Start Index")]
        [Default("0")]
        public InArgument<int> StartIndex { get; set; }

        [Input("Length")]
        [Default("3")]
        public InArgument<int> Length { get; set; }
        
    }
}