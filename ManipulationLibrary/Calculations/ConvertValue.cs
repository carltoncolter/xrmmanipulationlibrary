// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		ConvertValue.cs
//  Summary:	This workflow activity converts one value to another value
// ==================================================================================
using System;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Calculations
{
    [CrmWorkflowActivity("Convert Value", "Calculation Utilities")]
    public partial class ConvertValue : SequenceActivity
    {
        public ConvertValue()
        {
            InitializeComponent();
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string),
                                                                                     typeof(ConvertValue));
        [CrmInput("Value to convert")]
        [CrmDefault("0")]
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty DecProperty = DependencyProperty.Register("Dec", typeof(CrmDecimal), typeof(ConvertValue));
        [CrmOutput("Decimal")]
        public CrmDecimal Dec
        {
            get { return (CrmDecimal)GetValue(DecProperty); }
            set { SetValue(DecProperty, value); }
        }

        public static DependencyProperty MoneyProperty = DependencyProperty.Register("Money", typeof(CrmMoney), typeof(ConvertValue));
        [CrmOutput("Money")]
        public CrmMoney Money
        {
            get { return (CrmMoney)GetValue(MoneyProperty); }
            set { SetValue(MoneyProperty, value); }
        }

        public static DependencyProperty FloatProperty = DependencyProperty.Register("Float", typeof(CrmFloat), typeof(ConvertValue));
        [CrmOutput("Float")]
        public CrmFloat Float
        {
            get { return (CrmFloat)GetValue(FloatProperty); }
            set { SetValue(FloatProperty, value); }
        }

        public static DependencyProperty RoundedProperty = DependencyProperty.Register("Rounded", typeof(CrmNumber), typeof(ConvertValue));
        [CrmOutput("Rounded Number")]
        public CrmNumber Rounded
        {
            get { return (CrmNumber)GetValue(RoundedProperty); }
            set { SetValue(RoundedProperty, value); }
        }

        public static DependencyProperty TruncatedProperty = DependencyProperty.Register("Truncated", typeof(CrmNumber), typeof(ConvertValue));
        [CrmOutput("Truncated Number")]
        public CrmNumber Truncated
        {
            get { return (CrmNumber)GetValue(TruncatedProperty); }
            set { SetValue(TruncatedProperty, value); }
        }

        public static DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(CrmBoolean), typeof(ConvertValue));
        [CrmOutput("Formula Processing Error")]
        [CrmDefault("False")]
        public CrmBoolean Error
        {
            get { return (CrmBoolean)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }
               
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Error = new CrmBoolean { Value = false };
            try
            {
                var value = Convert.ToDouble(Value);
                
                Float = new CrmFloat {Value = value, formattedvalue = String.Format("{0:0.0}", value)};

                Dec = new CrmDecimal { Value = Convert.ToDecimal(Math.Round(value, 2)) };
                
                Money = new CrmMoney { Value = Convert.ToDecimal(Math.Round(value, 2)) };
                
                Truncated = new CrmNumber { Value = Convert.ToInt32(Math.Truncate(value)) };
                
                Rounded = new CrmNumber { Value = Convert.ToInt32(Math.Round(value, 0)) };
            }
            catch
            {
                Error.Value = true;
            }

            return base.Execute(executionContext);
        }
    }
}