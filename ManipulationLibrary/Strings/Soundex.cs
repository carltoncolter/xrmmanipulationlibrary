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
        /// <param name="limit">Maximum number of characters to process</param>
        /// <returns>The metaphone encoded string</returns>
        public static string Codify(string text, int min, int max, int limit=0)
        {
            if (String.IsNullOrEmpty(text)) return String.Empty;

            var str =
             (limit == 0) ? text.Trim() :
             text.Substring(0, limit > text.Length ? text.Length : limit);

            var soundex = new StringBuilder();

            const string source = "etaoinshrdlcumwfgypbvkjxqz";
            const string encode = "03000520634205012011122222";

            var firstChar = '!';
            var lastChar = '!';
            for (var i = 0; i < str.Length; i++)
            {
                if (firstChar == '!' && Char.IsLetter(str[i]) && str[i] < 128)
                    firstChar = str[i];

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
                if (max != -1 && soundex.Length >= max) break;
            }

            soundex.Remove(0, 1);
            soundex.Insert(0, Char.ToUpper(firstChar));

            while (soundex.Length < min) soundex.Append('0');

            if (soundex.Length > max) soundex.Remove(max, soundex.Length - max);

            return soundex.ToString();
        }
        
        protected override void Execute(CodeActivityContext executionContext)
        {
            var minLength = MinLength.Get<int>(executionContext);
            var maxLength = MaxLength.Get<int>(executionContext);
            var limit = Limit.Get<bool>(executionContext);
            var text = Text.Get<string>(executionContext);
            var result = Codify(text, minLength, maxLength, limit);
            Result.Set(executionContext,result);
        }

        [Input("Minimum Length")]
        [Default("4")]
        public InArgument<int> MinLength { get; set; }

        [Input("Maximum Length")]
        [Default("4")]
        public InArgument<int> MaxLength { get; set; }
        
        [Input("Limit")]
        [Default("4")]
        public InArgument<int> Limit { get; set; }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }
    }
}
