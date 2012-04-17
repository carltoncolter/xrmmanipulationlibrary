// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		Metaphone.cs
//  Summary:	This class does a form of metaphone codification. 
// ==================================================================================
/* 
 * Metaphone is a phonetic algorithm published in 1990 for indexing words by their 
 * English pronunciation. The original metaphone algorithm was developed by Lawrence 
 * Phillips who later created a double-metaphone algorithm to produce more accurate 
 * results.
 * 
 * This algorithm is an implementation somewhere between metaphone and 
 * double-metaphone.  It handles some of the special cases that are handled in 
 * double-metaphone, but still does a single codification.
 * 
 * Adam Nelson wrote a .Net metaphone and double-metaphone implementation, however, 
 * this is not Adam Nelson's code and does not match the logic in either of his 
 * implementations or Lawrence Phillips' algorithms.  It is a variant.
 * 
 * In this version, the metaphone codification is processed by funnelling through
 * a translation table.  If you would like to modify the translations, change the 
 * definitions in the static class.
 */
using System;
using System.Activities;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Strings
{
    public sealed class Metaphone : CodeActivity
    {
        private const int ComMaxLength = 6;

        private const int PreMaxLength = 3;
        private static readonly Dictionary<string, string> _commonTranslationTable;
        private static readonly Dictionary<string, string> _preTranslationTable;


        static Metaphone()
        {
            _preTranslationTable = new Dictionary<string, string>
                                  {
                                      {"ghi", "j"},
                                      {"a", "a"},
                                      {"e", "e"},
                                      {"i", "i"},
                                      {"o", "o"},
                                      {"u", "u"},
                                      {"ae", "e"},
                                      {"gn", "n"},
                                      {"kn", "n"},
                                      {"pn", "n"},
                                      {"x", "s"},
                                      {"y", "y"},
                                      {"h", "h"},
                                      {"sch", "sk"},
                                      {"slz", ""},
                                      {"sl", ""},
                                      {"wy", "y"},
                                      {"why", "y"},
                                      {"wa", "a"},
                                      {"we", "a"},
                                      {"wi", "i"},
                                      {"wo", "o"},
                                      {"wu", "u"},
                                      {"wh", "w"},
                                      {"wr", "r"}
                                  };

            _commonTranslationTable = new Dictionary<string, string>
                                     {
                                         {"b", "b"},
                                         {"bb", "b"},
                                         {"cq", "k"},
                                         {"cg", "k"},
                                         {"ck", "k"},
                                         {"cci", "x"},
                                         {"uccee", "ks"},
                                         {"uccess", "kss"},
                                         {"ucces", "kss"},
                                         {"cce", "x"},
                                         {"cch", "x"},
                                         {"cia", "x"},
                                         {"ci", "s"},
                                         {"mch", "mk"},
                                         {"mb", "m"},
                                         {"achl", "kl"},
                                         {"achr", "kr"},
                                         {"achn", "kn"},
                                         {"achm", "km"},
                                         {"achb", "kb"},
                                         {"achf", "kf"},
                                         {"achv", "kf"},
                                         {"achw", "k"},
                                         {"ochl", "kl"},
                                         {"ochr", "kr"},
                                         {"ochn", "kn"},
                                         {"ochm", "km"},
                                         {"ochb", "kb"},
                                         {"ochf", "kf"},
                                         {"ochv", "kf"},
                                         {"ochw", "k"},
                                         {"uchl", "kl"},
                                         {"uchr", "kr"},
                                         {"uchn", "kn"},
                                         {"uchm", "km"},
                                         {"uchb", "kb"},
                                         {"uchf", "kf"},
                                         {"uchv", "kf"},
                                         {"uchw", "k"},
                                         {"echl", "kl"},
                                         {"echr", "kr"},
                                         {"echn", "kn"},
                                         {"echm", "km"},
                                         {"echb", "kb"},
                                         {"echf", "kf"},
                                         {"echv", "kf"},
                                         {"echw", "k"},
                                         {"orches", "rks"},
                                         {"archit", "rkt"},
                                         {"orchid", "rkt"},
                                         {"chia", "k"},
                                         {"chae", "k"},
                                         {"chem", "k"},
                                         {"chym", "k"},
                                         {"chore", "kr"},
                                         {"choru", "kr"},
                                         {"charis", "krs"},
                                         {"charac", "krk"},
                                         {"cha", "x"},
                                         {"che", "x"},
                                         {"chi", "x"},
                                         {"cho", "x"},
                                         {"chu", "x"},
                                         {"ce", "s"},
                                         {"cy", "s"},
                                         {"cz", "z"},
                                         {"cc", "k"},
                                         {"c", "k"},
                                         {"dgy", "j"},
                                         {"dge", "j"},
                                         {"dgi", "j"},
                                         {"dg", "tk"},
                                         {"dt", "t"},
                                         {"dd", "t"},
                                         {"d", "t"},
                                         {"ff", "f"},
                                         {"f", "f"},
                                         {"gha", "k"},
                                         {"ghe", "k"},
                                         {"ghi", "k"},
                                         {"gho", "k"},
                                         {"ghu", "k"},
                                         {"ght", "t"},
                                         {"cough", "kf"},
                                         {"gough", "kf"},
                                         {"laugh", "lf"},
                                         {"rough", "rf"},
                                         {"tough", "tf"},
                                         {"gge", "j"},
                                         {"ggi", "j"},
                                         {"ggy", "j"},
                                         {"g", "k"},
                                         {"hour", "or"},
                                         {"heir", "r"},
                                         {"honor", "nr"},
                                         {"honest", "nst"},
                                         {"ah", "h"},
                                         {"eh", "h"},
                                         {"ih", "h"},
                                         {"oh", "h"},
                                         {"uh", "h"},
                                         {"jose", "js"},
                                         {"jj", "j"},
                                         {"j", "j"},
                                         {"kk", "k"},
                                         {"k", "k"},
                                         {"ll", "l"},
                                         {"l", "l"},
                                         {"mm", "m"},
                                         {"m", "m"},
                                         {"nn", "n"},
                                         {"n", "n"},
                                         {"ph", "f"},
                                         {"pp", "p"},
                                         {"pb", "p"},
                                         {"p", "p"},
                                         {"qq", "q"},
                                         {"q", "q"},
                                         {"rr", "r"},
                                         {"r", "r"},
                                         {"schoo", "sk"},
                                         {"schuy", "sk"},
                                         {"sched", "skt"},
                                         {"schem", "skm"},
                                         {"sch", "x"},
                                         {"sci", "s"},
                                         {"sce", "s"},
                                         {"scy", "s"},
                                         {"sc", "s"},
                                         {"sugar", "xkr"},
                                         {"sheim", "sm"},
                                         {"shoek", "sk"},
                                         {"sholm", "slm"},
                                         {"sholz", "slz"},
                                         {"isl", "l"},
                                         {"ysl", "l"},
                                         {"sz", "s"},
                                         {"ss", "s"},
                                         {"s", "s"},
                                         {"tio", "x"},
                                         {"tii", "x"},
                                         {"tih", "x"},
                                         {"tco", "x"},
                                         {"tci", "x"},
                                         {"tch", "x"},
                                         {"t", "t"},
                                         {"v", "f"},
                                         {"wr", "r"},
                                         {"wicz", "wts"},
                                         {"witz", "wts"},
                                         {"aux", ""},
                                         {"oux", ""},
                                         {"eaux", ""},
                                         {"x", "ks"},
                                         {"zh", "h"},
                                         {"zz", "s"},
                                         {"z", "s"}
                                     };
        }

        [Input("Maximum Length")]
        [Default("4")]
        public InArgument<int> MaxLength { get; set; }
        
        [Input("Minimum Length")]
        [Default("4")]
        public InArgument<int> MinLength { get; set; }

        [Input("Text")]
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            var text = Text.Get<string>(executionContext);
            var min = MinLength.Get<int>(executionContext);
            var max =  MaxLength.Get<int>(executionContext);
            var result = Codify(text, min, max);

            Result.Set(executionContext,result);
        }

        private static string Codify(string text, int min, int max)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            var translation = new StringBuilder();

            foreach (var s in text.ToLower().Split(' '))
            {
                var index = 0;
                for (var i = PreMaxLength; i > 0; i--)
                {
                    if (s.Length < i)
                    {
                        continue;
                    }

                    var key = s.Substring(index, i);
                    if (_preTranslationTable.ContainsKey(key))
                    {
                        translation.Append(_preTranslationTable[key]);
                        index += (key.Length - 1);
                    }
                }

                var last = s.Length;

                for (; index < last; index++)
                {
                    for (var i = ComMaxLength; i > 0; i--)
                    {
                        if (s.Length - index < i)
                        {
                            continue;
                        }

                        var key = s.Substring(index, i);
                        if (_commonTranslationTable.ContainsKey(key))
                        {
                            translation.Append(_commonTranslationTable[key]);
                            index += (key.Length - 1);
                        }
                    }

                    // exit if threshold reached
                    if (translation.Length >= max)
                    {
                        break;
                    }
                }
            }

            while (translation.Length < min)
            {
                translation.Append('0');
            }

            if (translation.Length > max && max != 0)
            {
                translation.Remove(max, translation.Length - max);
            }

            return translation.ToString().ToUpper();
        }
    }
}