// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Trim.cs
// ==================================================================================
using System;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Trim", "String Utilities")]
    public partial class Trim : SequenceActivity
    {
        public Trim()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Text = Text.Trim();

            return base.Execute(executionContext);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Trim));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
