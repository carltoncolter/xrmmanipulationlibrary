// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Match.cs
// ==================================================================================
using System;
using System.Text.RegularExpressions;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.RegEx
{
    [CrmWorkflowActivity("Match", "RegEx Utilities")]
    public partial class Match : SequenceActivity
    {
        public Match()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Invalid = new CrmBoolean { Value = false };
            Found = new CrmBoolean { Value = false };

            try
            {
                var regex = new Regex(Pattern);
                var result = regex.Match(Text);
                Found.Value = result.Success;
            }
            catch (ArgumentException)
            {
                Invalid.Value = true;
                // Syntax error in the regular expression
            }

            return base.Execute(executionContext);
        }

        public static DependencyProperty InvalidProperty = DependencyProperty.Register("Invalid", typeof(CrmBoolean), typeof(Match));
        [CrmOutput("Invalid Regular Expression")]
        [CrmDefault("False")]
        public CrmBoolean Invalid
        {
            get { return (CrmBoolean)GetValue(InvalidProperty); }
            set { SetValue(InvalidProperty, value); }
        }

        public static DependencyProperty FoundProperty = DependencyProperty.Register("Found", typeof(CrmBoolean), typeof(Match));
        [CrmOutput("Match Found")]
        [CrmDefault("False")]
        public CrmBoolean Found
        {
            get { return (CrmBoolean)GetValue(FoundProperty); }
            set { SetValue(FoundProperty, value); }
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Match));
        [CrmInput("Text")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(String), typeof(Match));
        [CrmInput("Regular Expression Pattern")]
        public String Pattern
        {
            get { return (String)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
    }
}
