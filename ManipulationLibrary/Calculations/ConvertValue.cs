// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		ConvertValue.cs
//  Summary:	This workflow activity converts one value to another value
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Calculations
{
    public sealed class ConvertValue : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var error = false;
            try
            {
                var value = Convert.ToDouble(Value.Get<string>(executionContext));

                FloatValue.Set(executionContext, value);

                DecimalValue.Set(executionContext, Convert.ToDecimal(Math.Round(value, 2)));

                MoneyValue.Set(executionContext, new Money { Value = Convert.ToDecimal(Math.Round(value, 2)) });

                TruncatedValue.Set(executionContext, Convert.ToInt32(Math.Truncate(value)));
                
                RoundedValue.Set(executionContext, Convert.ToInt32(Math.Round(value, 0)));
            }
            catch
            {
                error = true;
            }

            ProcessingError.Set(executionContext, error);
        }
        
        [Input("Value to convert")]
        [Default("0")]
        public InArgument<string> Value { get; set; }
        
        [Output("Decimal")]
        public OutArgument<decimal > DecimalValue { get; set; }
        
        [Output("Money")]
        public OutArgument<Money> MoneyValue { get; set; }
        
        [Output("Float")]
        public OutArgument<double> FloatValue { get; set; }

        [Output("Rounded Number")]
        public OutArgument<int> RoundedValue { get; set; }

        [Output("Truncated Number")]
        public OutArgument<int> TruncatedValue { get; set; }

        [Output("Processing Error")]
        [Default("False")]
        public OutArgument<bool> ProcessingError { get; set; }

    }
}