// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Substring.cs
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Substring", "String Utilities")]
    public partial class Substring : SequenceActivity
    {
        public Substring()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (Length.Value <= 0 || Start.Value < 0)
            {
                Text = String.Empty;
            }
            else
            {
                int start = Start.Value;
                if (!LeftToRight.Value)
                {
                    start = Text.Length - Length.Value - Start.Value;
                }
                Text = Text.Substring(start, Length.Value);
            }
            return base.Execute(executionContext);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Substring));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty LeftToRightProperty = DependencyProperty.Register("LeftToRight", typeof(CrmBoolean), typeof(Substring));
        [CrmInput("From Left To Right")]
        [CrmDefault("True")]
        public CrmBoolean LeftToRight
        {
            get { return (CrmBoolean)GetValue(LeftToRightProperty); }
            set { SetValue(LeftToRightProperty, value); }
        }

        public static DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(CrmNumber), typeof(Substring));
        [CrmInput("Start Index")]
        [CrmDefault("0")]
        public CrmNumber Start
        {
            get { return (CrmNumber)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(CrmNumber), typeof(Substring));
        [CrmInput("Length")]
        [CrmDefault("3")]
        public CrmNumber Length
        {
            get { return (CrmNumber)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

    }
}
