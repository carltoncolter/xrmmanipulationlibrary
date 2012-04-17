// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		BasicMath.cs
//  Summary:	This workflow activity solves basic math equations. 
// ==================================================================================

using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Calculations
{
    public sealed class BasicMath : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var n = Number1.Get<decimal>(executionContext);
            var n2 = Number2.Get<decimal>(executionContext);
            
            switch (Symbol.Get<string>(executionContext))
            {
                case "+":
                    n += n2;
                    break;
                case "-":
                    n -= n2;
                    break;
                case "/":
                    n /= n2;
                    break;
                case "*":
                    n *= n2;
                    break;
            }

            Result.Set(executionContext, n);
        }

        [Input("First number")]
        [Default("0")]
        public InArgument<decimal> Number1 { get; set; }

        [Input("Second number")]
        [Default("0")]
        public InArgument<decimal> Number2 { get; set; }

        [Input("Symbol")]
        [Default("+")]
        public InArgument<string> Symbol { get; set; }

        [Output("Result")]
        public OutArgument<decimal > Result { get; set; }
    }
}
