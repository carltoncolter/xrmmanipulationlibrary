// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Soundex.cs
//  Summary:	This class does a soundex string codification. 
// ==================================================================================
using System;
using System.Text;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Workflow;

namespace ManipulationLibrary.Strings
{
    [CrmWorkflowActivity("Codify (SoundEx)", "String Utilities")]
    public partial class Soundex : SequenceActivity
    {
        public Soundex()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convert string to a SoundEx Codified string
        /// </summary>
        /// <param name="text">The original text</param>
        /// <param name="min">The minimum length (0's will be appended to make up space)</param>
        /// <param name="max">The maximum length (string will be cut off)</param>
        /// <param name="original">Whether or not to use the original calculation method</param>
        /// <returns>The metaphone encoded string</returns>
        private static string Codify(string text, int min, int max, bool original)
        {
            if (String.IsNullOrEmpty(text)) return String.Empty;

            var str = text.Trim();

            var soundex = new StringBuilder();

            const string source = "etaoinshrdlcumwfgypbvkjxqz";

            var encode = "0300052D634205D12011122222";
            if (original)
                encode = "03000520634205012011122222";

            var lastChar = encode[source.IndexOf(Char.ToLower(str[0]))];
            for (var i = 1; i < str.Length; i++)
            {
                var codeIndex = source.IndexOf(Char.ToLower(str[i]));
                if (codeIndex < 0)
                {
                    continue;
                }

                var encodedChar = encode[codeIndex];

                if (encodedChar == '0' || encodedChar == lastChar)
                {
                    continue;
                }

                lastChar = encodedChar;
                soundex.Append(encodedChar);
            }

            soundex.Replace("D", String.Empty);
            soundex.Insert(0, Char.ToUpper(str[0]));

            while (soundex.Length < min) soundex.Append('0');

            if (soundex.Length > max) soundex.Remove(max, soundex.Length - max);

            return soundex.ToString();
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Text = Codify(Text, MinLength.Value, MaxLength.Value, UseOriginal.Value);
            return base.Execute(executionContext);
        }

        public static DependencyProperty MinLengthProperty = DependencyProperty.Register("MinLength", typeof(CrmNumber), typeof(Soundex));
        [CrmInput("Minimum Length")]
        [CrmDefault("4")]
        public CrmNumber MinLength
        {
            get { return (CrmNumber)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        public static DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(CrmNumber), typeof(Soundex));
        [CrmInput("Maximum Length")]
        [CrmDefault("4")]
        public CrmNumber MaxLength
        {
            get { return (CrmNumber)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static DependencyProperty UseOriginalProperty = DependencyProperty.Register("UseOriginal", typeof(CrmBoolean), typeof(Soundex));
        [CrmInput("Use Original Version")]
        [CrmDefault("false")]
        public CrmBoolean UseOriginal
        {
            get { return (CrmBoolean)GetValue(UseOriginalProperty); }
            set { SetValue(UseOriginalProperty, value); }
        }


        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Soundex));
        [CrmInput("Text")]
        [CrmOutput("Result")]
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
