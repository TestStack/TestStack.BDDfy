using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    internal static class ArgumentCleaningExtensions
    {
        internal static object[] FlattenArrays(this IEnumerable<object> inputs)
        {
            return [.. inputs.Select(FlattenArray)];
        }

        public static object FlattenArray(this object? input)
        {
            if (input is Array inputArray)
            {
                var temp = from object arrElement in inputArray select GetSafeValue(arrElement);
                return string.Join(", ", temp);
            }

            return GetSafeValue(input);
        }

        static object GetSafeValue(object? input) => input switch
        {
            null => "<null>",
            string s => s == "" ? "<empty>" : (s.Trim() == "" ? $"'{s}'" : s),
            _ => input
        };
    }
}