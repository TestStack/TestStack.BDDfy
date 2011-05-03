using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
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
                        if (lastCharacterInTheList != ' ')
                        {
                            if(char.IsDigit(lastCharacterInTheList))
                            {
                                if (char.IsLetter(currentChar))
                                    list.Add(' ');
                            }
                            else if (!char.IsLower(currentChar))
                                list.Add(' ');
                        }

                        list.Add(char.ToLower(currentChar));

                        return list;
                    });

            var result = string.Join("", chars);
            return result.Replace(" i ", " I "); // I is an exception
        }

        public static Func<string, string> Convert = name => FromPascalCase(FromUnderscoreSeparatedWords(name));
    }
}