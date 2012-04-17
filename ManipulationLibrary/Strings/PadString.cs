// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		PadString.cs
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class PadString : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var padchar = PadCharacter.Get<string>(executionContext);
            var pad = (String.IsNullOrEmpty(padchar)) ? ' ' : padchar[0];
            var length = FinalLength.Get<int>(executionContext);

            var text = Text.Get<string>(executionContext);
            var result = PadOnLeft.Get<bool>(executionContext) ? text.PadLeft(length, pad) : text.PadRight(length, pad);

            Result.Set(executionContext, result);
        }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

        [Input("Pad Character")]
        public InArgument<string> PadCharacter { get; set; }

        [Input("Pad on the Left")]
        [Default("True")]
        public InArgument<bool> PadOnLeft { get; set; }

        [Input("Final Length")]
        [Default("30")]
        public InArgument<int> FinalLength { get; set; }

    }
}
