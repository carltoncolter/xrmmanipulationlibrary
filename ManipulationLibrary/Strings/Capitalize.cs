// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Capitalize.cs
//  Summary:    Capitalize the beginning of words in a string
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Capitalize", "String Utilities")]
    public partial class Capitalize : SequenceActivity
    {
        public Capitalize()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (CapAll.Value)
            {
                // All words
                Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Text);
            } else
            {
                // First Letter only
                Text = Text.Substring(0, 1).ToUpper() + Text.Substring(1);
                
            }
            return base.Execute(executionContext);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Capitalize));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty CapAllProperty = DependencyProperty.Register("CapAll", typeof(CrmBoolean), typeof(Capitalize));
        [CrmInput("Capitalize All Words")]
        [CrmDefault("True")]
        public CrmBoolean CapAll
        {
            get { return (CrmBoolean)GetValue(CapAllProperty); }
            set { SetValue(CapAllProperty, value); }
        }
    }
}
