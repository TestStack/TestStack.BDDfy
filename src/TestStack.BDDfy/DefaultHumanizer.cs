using System;
using System.Linq;
using System.Text.RegularExpressions;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    internal partial class DefaultHumanizer: IHumanizer
    {
        private static readonly Regex ConsecutiveCapitalLetters = new("([A-Z]+)([A-Z][a-z])");
        private static readonly Regex TokensPattern = new("__([a-zA-Z0-9]+)__");
        private static readonly Regex PascalCaseRegex = new(@"(?<!^)([A-Z])");
        private static readonly Regex TokenReplacePattern = new("#(\\S+)#");
        private static readonly Regex SentenceWithNumerals = new(@"(?<=[a-zA-Z])(?=\d)(?![^<>]*>)");
        private static readonly Regex UnicodeMatchPattern = new(@"[^\u0000-\u007F]");
        private static readonly Regex LoneIReplacePattern = new(@"(?<=^|\s)i(?=\s|$)");

        public string Humanize(string input)
        {
            var shouldPreserveCasing = input.Replace("__", "-").Contains('_');

            input = TokensPattern.Replace(input, "-#$1#-");

            var words = input.Split(['_','-']);

            var finalWords = words.Select(x => TokenReplacePattern.Replace(x, "<$1>"));

            var sentence = string.Join(" ", finalWords);

            if (!shouldPreserveCasing)
            {
                sentence = ConsecutiveCapitalLetters.Replace(sentence, "$1 $2");
                sentence = PascalToSentence(sentence);
                sentence = SentenceWithNumerals.Replace(sentence, " ");
                if (UnicodeMatchPattern.IsMatch(input)) 
                    throw new ArgumentException("Non ascii characters detected");
            }
            
            
            sentence = sentence.Trim().Replace("  "," ");
            sentence = LoneIReplacePattern.Replace(sentence, "I");
            return sentence;
        }

        public static string PascalToSentence(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sentence = PascalCaseRegex.Replace(input, " $1");
            sentence = sentence.Replace("< ", "<").Replace(" >",">");
            var final = char.ToUpper(sentence[0]) + sentence[1..].ToLower();
            return final;
        }
    }
}
