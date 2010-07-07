// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		BasicMath.cs
//  Summary:	This workflow activity solves basic math equations. 
// ==================================================================================

using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Calculations
{
    [CrmWorkflowActivity("Basic Math", "Calculation Utilities")]
    public partial class BasicMath : SequenceActivity
    {
        public BasicMath()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            
            switch (Symbol)
            {
                case "+":
                    Number.Value += Number2.Value;
                    break;
                case "-":
                    Number.Value -= Number2.Value;
                    break;
                case "/":
                    Number.Value /= Number2.Value;
                    break;
                case "*":
                    Number.Value *= Number2.Value;
                    break;
            }

            return base.Execute(executionContext);
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(CrmDecimal), typeof(BasicMath));
        [CrmInput("First number")]
        [CrmOutput("Result")]
        public CrmDecimal Number
        {
            get { return (CrmDecimal)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }


        public static DependencyProperty Number2Property = DependencyProperty.Register("Number2", typeof(CrmDecimal), typeof(BasicMath));
        [CrmInput("Second number")]
        public CrmDecimal Number2
        {
            get { return (CrmDecimal)GetValue(Number2Property); }
            set { SetValue(Number2Property, value); }
        }

        public static DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(string), typeof(BasicMath));
        [CrmInput("Symbol")]
        [CrmDefault("+")]
        public string Symbol
        {
            get { return (string)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }
        
    }
}
