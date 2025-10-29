using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    internal partial class DefaultHumanizer: IHumanizer
    {
        private static readonly TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        public Func<string, bool> PreserveCasingWhen { get; set; } = input
            => input.Replace("__", "-").Contains('_');

        public string Humanize(string input)
        {
            var shouldPreserveCasing = PreserveCasingWhen(input);
            input = TokensPattern().Replace(input, "-#$1#-");

            var words = input.Split(['_','-']);

            var finalWords = words.Select(x => TokenReplacePattern().Replace(x, "<$1>"));

            var sentence = string.Join(" ", finalWords);

            if (!shouldPreserveCasing)
            {
                sentence = ConsecutiveCapitalLetters().Replace(sentence, "$1 $2");
                sentence = PascalToSentence(sentence);
                sentence = SentenceWithNumerals().Replace(sentence, " ");
                if (UnicodeMatchPattern().IsMatch(input)) 
                    throw new ArgumentException("Non ascii characters detected");
            }
            
            
            sentence = sentence.Trim().Replace("  "," ");
            sentence = LoneIReplacePattern().Replace(sentence, "I");
            return sentence;
        }

        public static string PascalToSentence(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sentence = PascalCaseRegex().Replace(input, " $1");
            sentence = sentence.Replace("< ", "<").Replace(" >",">");
            var final = char.ToUpper(sentence[0]) + sentence.Substring(1).ToLower();
            return final;
        }


        [GeneratedRegex("([A-Z]+)([A-Z][a-z])")]
        private static partial Regex ConsecutiveCapitalLetters();

        [GeneratedRegex("__([a-zA-Z0-9]+)__")]
        private static partial Regex TokensPattern();

        [GeneratedRegex(@"(?<!^)([A-Z])")]
        private static partial Regex PascalCaseRegex();

        [GeneratedRegex("#(\\S+)#")]
        private static partial Regex TokenReplacePattern();

        [GeneratedRegex(@"(?<=[a-zA-Z])(?=\d)(?![^<>]*>)")]
        private static partial Regex SentenceWithNumerals();

        [GeneratedRegex(@"[^\u0000-\u007F]")]
        private static partial Regex UnicodeMatchPattern();

        [GeneratedRegex(@"(?<=^|\s)i(?=\s|$)")]
        private static partial Regex LoneIReplacePattern();
    }
}
