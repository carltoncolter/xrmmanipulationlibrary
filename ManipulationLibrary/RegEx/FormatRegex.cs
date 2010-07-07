// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		FormatRegex.cs
//  Summary:	This workflow activity formats a string using regular expression
// ==================================================================================
using System;
using System.Text.RegularExpressions;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.RegEx
{
    [CrmWorkflowActivity("Format Matched String", "RegEx Utilities")]
    public partial class FormatRegex : SequenceActivity
    {
        public FormatRegex()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Invalid = new CrmBoolean { Value = false };
            Match = new CrmBoolean { Value = false };

            try
            {
                var regex = new Regex(Pattern);
                Match.Value = regex.IsMatch(Text);
                if (Match.Value)
                {
                    Text = regex.Replace(Text, Format);
                }
            }
            catch (ArgumentException)
            {
                // Syntax error in the regular expression
                Invalid.Value = true;
            }

            return base.Execute(executionContext);
        }

        public static DependencyProperty InvalidProperty = DependencyProperty.Register("Invalid", typeof(CrmBoolean), typeof(FormatRegex));
        [CrmOutput("Invalid Regular Expression")]
        [CrmDefault("False")]
        public CrmBoolean Invalid
        {
            get { return (CrmBoolean)GetValue(InvalidProperty); }
            set { SetValue(InvalidProperty, value); }
        }

        public static DependencyProperty MatchProperty = DependencyProperty.Register("Match", typeof(CrmBoolean), typeof(FormatRegex));
        [CrmOutput("Match Found")]
        [CrmDefault("False")]
        public CrmBoolean Match
        {
            get { return (CrmBoolean)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FormatRegex));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(String), typeof(FormatRegex));
        [CrmInput("Format")]
        [CrmDefault("(${area}) ${prefix}-${number}")]
        public String Format
        {
            get { return (String)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(String), typeof(FormatRegex));
        [CrmInput("Regular Expression Pattern")]
        [CrmDefault("^1?-?\\(?(?<area>[0-9]{3})\\)?[-. ]?(?<prefix>[0-9]{3})[-. ]?(?<number>[0-9]{4})$")]
        public String Pattern
        {
            get { return (String)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
    }
}
