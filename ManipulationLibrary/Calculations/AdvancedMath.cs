// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		AdvancedMath.cs
//  Summary:	This workflow activity solves math equations.
// ==================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using ManipulationLibrary.Calculations.Helpers;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Calculations
{
    [CrmWorkflowActivity("Solve Equation", "Calculation Utilities")]
    public partial class AdvancedMath : SequenceActivity
    {

        public AdvancedMath()
        {
            InitializeComponent();
        }
        
        private void AddParameters(Dictionary<string, string> parameters)
        {
            AddParameter(parameters, "@a", Var1);
            AddParameter(parameters, "@b", Var2);
            AddParameter(parameters, "@c", Var3);
            AddParameter(parameters, "@d", Var4);
            AddParameter(parameters, "@e", Var5);
            AddParameter(parameters, "@f", Var6);
            AddParameter(parameters, "@g", Var7);
            AddParameter(parameters, "@h", Var8);
            AddParameter(parameters, "@i", Var9);
            AddParameter(parameters, "@x", Var10);
            AddParameter(parameters, "@y", Var11);
            AddParameter(parameters, "@z", Var12);
        }

        private void AddParameter(Dictionary<string, string> parameters, string param, string variable)
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

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Error = new CrmBoolean { Value=false};
            ErrorMessage = String.Empty;
            MoneyOutput = new CrmMoney();
            Truncated = new CrmNumber();
            Rounded = new CrmNumber();
            FloatOutput = new CrmFloat();

            try
            {
                var parameters = new Dictionary<string, string>();
                AddParameters(parameters);
                var equation = parameters.Aggregate(Formula, (c, p) => c.Replace(p.Key, String.Format(" {0} ", p.Value)));
                SetOutputValues(Equation.Solve(equation));
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Error.Value = true;
            }

            return base.Execute(executionContext);
        }

        private void SetOutputValues(double value)
        {
            Var1 = String.Format("{0:0.0#####}", value);
            MoneyOutput.Value = Convert.ToDecimal(Math.Round(value, 2));
            Truncated.Value = Convert.ToInt32(Math.Truncate(value));
            Rounded.Value = Convert.ToInt32(Math.Round(value, 0));
            FloatOutput.Value = value;
            FloatOutput.formattedvalue = String.Format("{0:0.0#####}", value);
        }

        public static DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(CrmBoolean), typeof(AdvancedMath));
        [CrmOutput("Error Processing Formula")]
        [CrmDefault("False")]
        public CrmBoolean Error
        {
            get { return (CrmBoolean)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }

        public static DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(String), typeof(AdvancedMath));
        [CrmOutput("Error Message")]
        public String ErrorMessage
        {
            get { return (String)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static DependencyProperty FormulaProperty = DependencyProperty.Register("Formula", typeof(String), typeof(AdvancedMath));
        [CrmInput("Formula")]
        [CrmDefault("((3+5)/@a)*pi")]
        public String Formula
        {
            get { return (String)GetValue(FormulaProperty); }
            set { SetValue(FormulaProperty, value); }
        }

        public static DependencyProperty Var1Property = DependencyProperty.Register("Var1", typeof(string), typeof(AdvancedMath));
        [CrmInput("@a")]
        [CrmOutput("String Result")]
        public string Var1
        {
            get { return (string)GetValue(Var1Property); }
            set { SetValue(Var1Property, value); }
        }

        public static DependencyProperty Var2Property = DependencyProperty.Register("Var2", typeof(string), typeof(AdvancedMath));
        [CrmInput("@b")]
        public string Var2
        {
            get { return (string)GetValue(Var2Property); }
            set { SetValue(Var2Property, value); }
        }

        public static DependencyProperty Var3Property = DependencyProperty.Register("Var3", typeof(string), typeof(AdvancedMath));
        [CrmInput("@c")]
        public string Var3
        {
            get { return (string)GetValue(Var3Property); }
            set { SetValue(Var3Property, value); }
        }

        public static DependencyProperty Var4Property = DependencyProperty.Register("Var4", typeof(string), typeof(AdvancedMath));
        [CrmInput("@d")]
        public string Var4
        {
            get { return (string)GetValue(Var4Property); }
            set { SetValue(Var4Property, value); }
        }

        public static DependencyProperty Var5Property = DependencyProperty.Register("Var5", typeof(string), typeof(AdvancedMath));
        [CrmInput("@e")]
        public string Var5
        {
            get { return (string)GetValue(Var5Property); }
            set { SetValue(Var5Property, value); }
        }

        public static DependencyProperty Var6Property = DependencyProperty.Register("Var6", typeof(string), typeof(AdvancedMath));
        [CrmInput("@f")]
        public string Var6
        {
            get { return (string)GetValue(Var6Property); }
            set { SetValue(Var6Property, value); }
        }

        public static DependencyProperty Var7Property = DependencyProperty.Register("Var7", typeof(string), typeof(AdvancedMath));
        [CrmInput("@g")]
        public string Var7
        {
            get { return (string)GetValue(Var7Property); }
            set { SetValue(Var7Property, value); }
        }

        public static DependencyProperty Var8Property = DependencyProperty.Register("Var8", typeof(string), typeof(AdvancedMath));
        [CrmInput("@h")]
        public string Var8
        {
            get { return (string)GetValue(Var8Property); }
            set { SetValue(Var8Property, value); }
        }

        public static DependencyProperty Var9Property = DependencyProperty.Register("Var9", typeof(string), typeof(AdvancedMath));
        [CrmInput("@i")]
        public string Var9
        {
            get { return (string)GetValue(Var9Property); }
            set { SetValue(Var9Property, value); }
        }

        public static DependencyProperty Var10Property = DependencyProperty.Register("Var10", typeof(string), typeof(AdvancedMath));
        [CrmInput("@x")]
        public string Var10
        {
            get { return (string)GetValue(Var10Property); }
            set { SetValue(Var10Property, value); }
        }

        public static DependencyProperty Var11Property = DependencyProperty.Register("Var11", typeof(string), typeof(AdvancedMath));
        [CrmInput("@y")]
        public string Var11
        {
            get { return (string)GetValue(Var11Property); }
            set { SetValue(Var11Property, value); }
        }

        public static DependencyProperty Var12Property = DependencyProperty.Register("Var12", typeof(string), typeof(AdvancedMath));
        [CrmInput("@z")]
        public string Var12
        {
            get { return (string)GetValue(Var12Property); }
            set { SetValue(Var12Property, value); }
        }

        public static DependencyProperty MoneyOutputProperty = DependencyProperty.Register("MoneyOutput", typeof(CrmMoney), typeof(AdvancedMath));
        [CrmOutput("Money Result")]
        public CrmMoney MoneyOutput
        {
            get { return (CrmMoney)GetValue(MoneyOutputProperty); }
            set { SetValue(MoneyOutputProperty, value); }
        }

        public static DependencyProperty TruncatedProperty = DependencyProperty.Register("Truncated", typeof(CrmNumber), typeof(AdvancedMath));
        [CrmOutput("Truncated Number Result")]
        public CrmNumber Truncated
        {
            get { return (CrmNumber)GetValue(TruncatedProperty); }
            set { SetValue(TruncatedProperty, value); }
        }

        public static DependencyProperty RoundedProperty = DependencyProperty.Register("Rounded", typeof(CrmNumber), typeof(AdvancedMath));
        [CrmOutput("Rounded Number Result")]
        public CrmNumber Rounded
        {
            get { return (CrmNumber)GetValue(RoundedProperty); }
            set { SetValue(RoundedProperty, value); }
        }

        public static DependencyProperty FloatOutputProperty = DependencyProperty.Register("FloatOutput", typeof(CrmFloat), typeof(AdvancedMath));
        [CrmOutput("Float Result")]
        public CrmFloat FloatOutput
        {
            get { return (CrmFloat)GetValue(FloatOutputProperty); }
            set { SetValue(FloatOutputProperty, value); }
        }
    }
}
