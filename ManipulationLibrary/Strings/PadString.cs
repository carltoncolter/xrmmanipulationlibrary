// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		PadString.cs
// ==================================================================================
using System;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Pad String", "String Utilities")]
    public partial class PadString : SequenceActivity
    {
        public PadString()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var pad = (String.IsNullOrEmpty(PadCharacter)) ? ' ' : PadCharacter[0];
            
            Text = PadOnLeft.Value ? Text.PadLeft(Length.Value, pad) : Text.PadRight(Length.Value, pad);
            
            return base.Execute(executionContext);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(PadString));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty PadCharacterProperty = DependencyProperty.Register("PadCharacter", typeof(string), typeof(PadString));
        [CrmInput("Pad Character")]
        public string PadCharacter
        {
            get { return (string)GetValue(PadCharacterProperty); }
            set { SetValue(PadCharacterProperty, value); }
        }

        public static DependencyProperty PadOnLeftProperty = DependencyProperty.Register("PadOnLeft", typeof(CrmBoolean), typeof(PadString));
        [CrmInput("Pad on the Left")]
        [CrmDefault("True")]
        public CrmBoolean PadOnLeft
        {
            get { return (CrmBoolean)GetValue(PadOnLeftProperty); }
            set { SetValue(PadOnLeftProperty, value); }
        }

        public static DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(CrmNumber), typeof(PadString));
        [CrmInput("Final Length")]
        [CrmDefault("30")]
        public CrmNumber Length
        {
            get { return (CrmNumber)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

    }
}
