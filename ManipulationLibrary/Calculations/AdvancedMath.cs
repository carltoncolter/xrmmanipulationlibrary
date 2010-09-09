// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AdvancedMath.cs
//  Summary:	This workflow activity solves math equations.
// ==================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Activities;
using ManipulationLibrary.Calculations.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Calculations
{
    [WorkflowActivity("Solve Equation", "Calculation Utilities")]
    public sealed class AdvancedMath : CodeActivity
    {
        private void AddParameters(ActivityContext context, Dictionary<string, string> parameters)
        {
            AddParameter(parameters, "@a", Var1.Get<string>(context));
            AddParameter(parameters, "@b", Var2.Get<string>(context));
            AddParameter(parameters, "@c", Var3.Get<string>(context));
            AddParameter(parameters, "@d", Var4.Get<string>(context));
            AddParameter(parameters, "@e", Var5.Get<string>(context));
            AddParameter(parameters, "@f", Var6.Get<string>(context));
            AddParameter(parameters, "@g", Var7.Get<string>(context));
            AddParameter(parameters, "@h", Var8.Get<string>(context));
            AddParameter(parameters, "@i", Var9.Get<string>(context));
            AddParameter(parameters, "@x", Var10.Get<string>(context));
            AddParameter(parameters, "@y", Var11.Get<string>(context));
            AddParameter(parameters, "@z", Var12.Get<string>(context));
        }

        private static void AddParameter(IDictionary<string, string> parameters, string param, string variable)
        {
            if (String.IsNullOrEmpty(variable))
            {
                return;
            }

            // You can remove this double check to allow formulas to be entered as variables (parameters)
            double value;
            if (Double.TryParse(variable, out value))
            {

                parameters.Add(param, value.ToString());
            }
        }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var error = false;
            var errorMessage = String.Empty;
            var formula = Formula.Get<string>(executionContext);
            
            try
            {
                var parameters = new Dictionary<string, string>();
                AddParameters(executionContext, parameters);
                var equation = parameters.Aggregate(formula, (c, p) => c.Replace(p.Key, String.Format(" {0} ", p.Value)));
                
                SetOutputValues(executionContext, Equation.Solve(equation));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                error = true;
            }

            Error.Set(executionContext, error);
            ErrorMessage.Set(executionContext, errorMessage);
        }

        private void SetOutputValues(ActivityContext context, double value)
        {
            StringOutput.Set(context, String.Format("{0:0.0#####}", value));
            MoneyOutput.Set(context, new Money { Value = Convert.ToDecimal(Math.Round(value, 2)) });
            TruncatedOutput.Set(context, Convert.ToInt32(Math.Truncate(value)));
            RoundedOutput.Set(context, Math.Round(value, 0));
            FloatOutput.Set(context, value);
        }

        [Output("Error Processing Formula")]
        [Default("False")]
        public OutArgument<bool> Error { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }

        [Input("Formula")]
        [Default("((3+5)/@a)*pi")]
        public InArgument<string> Formula { get; set; }

        [Input("@a")]
        public InArgument<string> Var1 { get; set; }

        [Input("@b")]
        public InArgument<string> Var2 { get; set; }

        [Input("@c")]
        public InArgument<string> Var3 { get; set; }

        [Input("@d")]
        public InArgument<string> Var4 { get; set; }

        [Input("@e")]
        public InArgument<string> Var5 { get; set; }

        [Input("@f")]
        public InArgument<string> Var6 { get; set; }

        [Input("@g")]
        public InArgument<string> Var7 { get; set; }

        [Input("@h")]
        public InArgument<string> Var8 { get; set; }

        [Input("@i")]
        public InArgument<string> Var9 { get; set; }

        [Input("@x")]
        public InArgument<string> Var10 { get; set; }

        [Input("@y")]
        public InArgument<string> Var11 { get; set; }

        [Input("@z")]
        public InArgument<string> Var12 { get; set; }

        [Output("String Output")]
        public OutArgument<string> StringOutput { get; set; }

        [Output("Money Result")]
        public OutArgument<Money> MoneyOutput { get; set; }

        [Output("Truncated Number Output")]
        public OutArgument<int> TruncatedOutput { get; set; }

        [Output("Rounded Number Output")]
        public OutArgument<int> RoundedOutput { get; set; }

        [Output("Float Output")]
        public OutArgument<double> FloatOutput { get; set; }
    }
}
