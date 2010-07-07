// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Minimum.cs
//  Summary:	This workflow activity returns the lowest number between two objects
// ==================================================================================
using System;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Calculations
{
    [CrmWorkflowActivity("Minimum", "Calculation Utilities")]
    public partial class Minimum : SequenceActivity
    {
        public Minimum()
        {
            InitializeComponent();
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(CrmDecimal),
                                                                                     typeof(Minimum));
        [CrmInput("First number")]
        [CrmOutput("Result")]
        [CrmDefault("0")]
        public CrmDecimal Number
        {
            get { return (CrmDecimal)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static DependencyProperty Number2Property = DependencyProperty.Register("Number2", typeof(CrmDecimal),
                                                                                       typeof(Minimum));
        [CrmInput("Second number")]
        [CrmDefault("0")]
        public CrmDecimal Number2
        {
            get { return (CrmDecimal)GetValue(Number2Property); }
            set { SetValue(Number2Property, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Number.Value = Math.Min(Number.Value, Number2.Value);

            return base.Execute(executionContext);
        }
    }
}