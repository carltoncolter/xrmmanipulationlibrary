// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		URLEncode.cs
//  Summary:    URL Encodes a string
// ==================================================================================
using System.Activities;
using System.Web;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Web
{
    public sealed class URLEncode : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var text = Text.Get<string>(executionContext);

            var result = HttpUtility.UrlEncode(text);

            Result.Set(executionContext,result);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
