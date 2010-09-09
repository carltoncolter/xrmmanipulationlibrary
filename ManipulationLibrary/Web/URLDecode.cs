// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		URLdecode.cs
//  Summary:    URL Decodes a string
// ==================================================================================
using System.Activities;
using System.Web;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Web
{
    public sealed class URLDecode : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var text = Text.Get<string>(executionContext);

            var result = HttpUtility.UrlDecode(text);

            Result.Set(executionContext,result);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
