using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestStack.BDDfy
{
    public class NetToString
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

        public static readonly Func<string, string> Convert = name =>
        {
            if (name.Contains("__"))
                return ExampleTitle(name);

            if (name.Contains("_"))
                return FromUnderscoreSeparatedWords(name);

            return FromPascalCase(name);
        };

        private static string ExampleTitle(string name)
        {
            name = Regex.Replace(name, "__([a-zA-Z]+)__", " <$1> ");

            // for when there are two consequetive example placeholders in the word; e.g. Given__one____two__parameters
            name = name.Replace("  ", " ");
            return Convert(name).Trim();
        }
    }
}