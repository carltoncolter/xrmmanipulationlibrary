// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		StrLength.cs
//  Summary:    Get the length of a string.
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Length", "String Utilities")]
    public partial class StrLength : SequenceActivity
    {
        public StrLength()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Length = new CrmNumber { Value = Text.Length };
            
            return base.Execute(executionContext);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(StrLength));
        [CrmInput("Text")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(CrmNumber), typeof(StrLength));
        [CrmOutput("Length")]
        public CrmNumber Length
        {
            get { return (CrmNumber)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

    }
}
