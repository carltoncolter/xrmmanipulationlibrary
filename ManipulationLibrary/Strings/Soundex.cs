// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		Soundex.cs
//  Summary:	This class does a soundex string codification. 
// ==================================================================================
using System;
using System.Activities;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Soundex : CodeActivity
    {
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
        
        protected override void Execute(CodeActivityContext executionContext)
        {
            var minLength = MinLength.Get<int>(executionContext);
            var maxLength = MaxLength.Get<int>(executionContext);
            var useOriginalMethod = UseOriginal.Get<bool>(executionContext);
            var text = Text.Get<string>(executionContext);
            var result = Codify(text, minLength, maxLength, useOriginalMethod);
            Result.Set(executionContext,result);
        }

        [Input("Minimum Length")]
        [Default("4")]
        public InArgument<int> MinLength { get; set; }

        [Input("Maximum Length")]
        [Default("4")]
        public InArgument<int> MaxLength { get; set; }

        [Input("Use Original Version")]
        [Default("false")]
        public InArgument<bool> UseOriginal { get; set; }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
