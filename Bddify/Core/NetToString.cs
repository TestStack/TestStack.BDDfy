using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class NetToString
    {
        static Func<string, string> FromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        static string FromPascalCase(string name)
        {
            var chars = name.Aggregate(
                new List<char>(), 
                (list, currentChar) =>
                    {
                        if (currentChar == ' ')
                        {
                            list.Add(currentChar);
                            return list;
                        }

                        if(list.Count == 0)
                        {
                            list.Add(currentChar);
                            return list;
                        }

                        var lastCharacterInTheList = list[list.Count - 1];
                        if(char.IsLower(lastCharacterInTheList) && !char.IsLower(currentChar))
                            list.Add(' ');

                        if(char.IsDigit(lastCharacterInTheList) && char.IsLetter(currentChar))
                            list.Add(' ');

                        list.Add(char.ToLower(currentChar));

                        return list;
                    });

            var result = string.Join("", chars);
            return result.Replace(" i ", " I "); // I is an exception
        }

        public static Func<string, string> Convert = name => FromPascalCase(FromUnderscoreSeparatedWords(name));
    }
}