// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Replace.cs
// ==================================================================================
using System;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Replace", "String Utilities")]
    public partial class Replace : SequenceActivity
    {
        public Replace()
        {
            InitializeComponent();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (!CaseSensitive.Value)
            {
                if (!String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(Old))
                {
                    Text = Text.Replace(Old, New);
                }
            } else
            {
                Text = CompareAndReplace(Text, Old, New, StringComparison.CurrentCultureIgnoreCase);
            }

            return base.Execute(executionContext);
        }

        private static string CompareAndReplace (string text, string old, string @new, StringComparison comparison)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(old)) return text;

            var result = new StringBuilder();
            var oldLength = old.Length;
            var pos = 0;
            var next = text.IndexOf(old, comparison);

            while (next>0)
            {
                result.Append(text, pos, next - pos);
                result.Append(@new);
                pos = next + oldLength;
                next = text.IndexOf(old, pos, comparison);
            }

            result.Append(text, pos, text.Length - pos);
            return result.ToString();
        }
        
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Replace));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty OldProperty = DependencyProperty.Register("Old", typeof(String), typeof(Replace));
        [CrmInput("Old Value")]
        public String Old
        {
            get { return (String)GetValue(OldProperty); }
            set { SetValue(OldProperty, value); }
        }

        public static DependencyProperty NewProperty = DependencyProperty.Register("New", typeof(String), typeof(Replace));
        [CrmInput("New Value")]
        public String New
        {
            get { return (String)GetValue(NewProperty); }
            set { SetValue(NewProperty, value); }
        }

        public static DependencyProperty CaseSensitiveProperty = DependencyProperty.Register("CaseSensitive", typeof(CrmBoolean), typeof(Replace));
        [CrmInput("Case Sensitive")]
        [CrmDefault("False")]
        public CrmBoolean CaseSensitive
        {
            get { return (CrmBoolean)GetValue(CaseSensitiveProperty); }
            set { SetValue(CaseSensitiveProperty, value); }
        }
    }
}
