// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		ReturnMatch.cs
// ==================================================================================
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.RegEx
{
    [CrmWorkflowActivity("Return Match", "RegEx Utilities")]
    public partial class ReturnMatch : SequenceActivity
    {
        public ReturnMatch()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Invalid = new CrmBoolean { Value = false };
            Match = new CrmBoolean { Value = false };

            try
            {
                var result = new StringBuilder();
                var regex = new Regex(Pattern);

                var results = regex.Match(Text);
                var pos = 1;

                Match.Value = results.Success;

                while (results.Success)
                {
                    if (pos==Index.Value)
                    {
                        result.Append(results.Value);
                        break;
                    } 
                    
                    if (pos==-1 && Index.Value>=1)
                    {
                        result.Append(results.Value);
                    }
                    
                    results = results.NextMatch();
                    pos++;
                }

                if (result.Length>0)
                    Text = result.ToString();
            }
            catch (ArgumentException)
            {
                Invalid.Value = true;
                // Syntax error in the regular expression
            }

            return base.Execute(executionContext);
        }

        public static DependencyProperty InvalidProperty = DependencyProperty.Register("Invalid", typeof(CrmBoolean), typeof(ReturnMatch));
        [CrmOutput("Invalid Regular Expression")]
        [CrmDefault("False")]
        public CrmBoolean Invalid
        {
            get { return (CrmBoolean)GetValue(InvalidProperty); }
            set { SetValue(InvalidProperty, value); }
        }

        public static DependencyProperty MatchProperty = DependencyProperty.Register("Match", typeof(CrmBoolean), typeof(ReturnMatch));
        [CrmOutput("Match Found")]
        [CrmDefault("False")]
        public CrmBoolean Match
        {
            get { return (CrmBoolean)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        public static DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof (CrmNumber), typeof (ReturnMatch));
        [CrmInput("Match Index (-1 for all, 1 for first)")]
        [CrmDefault("-1")]
        public CrmNumber Index
        {
            get { return (CrmNumber)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }



        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(ReturnMatch));
        [CrmInput("Text")]
        [CrmOutput("Match")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(String), typeof(ReturnMatch));
        [CrmInput("Regular Expression Pattern")]
        public String Pattern
        {
            get { return (String)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
    }
}
