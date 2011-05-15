// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
//  File:		Soundex.cs
//  Summary:	This class does a soundex string codification. 
// ==================================================================================
using System;
using System.Text;

namespace SoundsLike
{
    public sealed class Soundex
    {
        // var result = Codify(text, minLength, maxLength, useOriginalMethod);

        /// <summary>
        /// Convert string to a SoundEx Codified string
        /// </summary>
        /// <param name="text">The original text</param>
        /// <param name="min">The minimum length (0's will be appended to make up space)</param>
        /// <param name="max">The maximum length (string will be cut off)</param>
        /// <param name="original">Whether or not to use the original calculation method</param>
        /// <returns>The metaphone encoded string</returns>
        public static string Codify(string text, int min, int max, bool original)
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
        
    }
}
