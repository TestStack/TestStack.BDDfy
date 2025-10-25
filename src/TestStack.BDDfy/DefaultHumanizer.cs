using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    internal partial class DefaultHumanizer: IHumanizer
    {
        static readonly Func<string, string> FromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        static string FromPascalCase(string name)
        {
            var chars = name.Aggregate(
                new List<char>(), 
                (list, currentChar) =>
                    {
                        if (currentChar == ' ' || list.Count == 0)
                            list.Add(currentChar);
                        else
                        {
                            if(ShouldAddSpace(list[list.Count - 1], currentChar))
                                list.Add(' ');
                            list.Add(char.ToLower(currentChar));
                        }

                        return list;
                    });

            var result = new string(chars.ToArray());
            return result.Replace(" i ", " I "); // I is an exception
        }

        private static bool ShouldAddSpace(char lastChar, char currentChar)
        {
            if (lastChar == ' ') 
                return false;

            if (char.IsDigit(lastChar))
            {
                if (char.IsLetter(currentChar))
                    return true;

                return false;
            }

            if (!char.IsLower(currentChar) && currentChar != '>' && lastChar != '<')
                return true;

            return false;
        }

        private static readonly Func<string, string> ConvertNonExample = name => {
            if (name.Contains('_'))
                return FromUnderscoreSeparatedWords(name);

            return FromPascalCase(name);
        };

        private string ExampleTitle(string name)
        {
            // Compare contains("__") with a regex match
            string newName = TitleCleanerRegex().Replace(name, " <$1> ");

            if (newName == name) {
                throw new ArgumentException("Illegal example title in name '" + name + "'!");
            }

            // for when there are two consequetive example placeholders in the word; e.g. Given__one____two__parameters
            newName = newName.Replace("  ", " ");
            return Humanize(newName).Trim();
        }

        public string Humanize(string input) 
            => input.Contains("__") ? ExampleTitle(input) : ConvertNonExample(input);

        [GeneratedRegex("__([a-zA-Z][a-zA-Z0-9]*)__")]
        private static partial Regex TitleCleanerRegex();
    }
}