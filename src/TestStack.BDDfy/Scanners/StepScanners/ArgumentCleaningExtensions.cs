using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    internal static class ArgumentCleaningExtensions
    {
        public const string NullValueRepresentation = "<null>";
        internal static object[] FlattenArrays(this IEnumerable<object> inputs)
        {
            return [.. inputs.Select(FlattenArray)];
        }

        public static object FlattenArray(this object? input)
        {
            if (input is Array inputArray)
            {
                var temp = from object arrElement in inputArray select arrElement.GetSafeValue();
                return string.Join(", ", temp);
            }

            return input.GetSafeValue();
        }

        private static object GetSafeValue(this object? input) => input switch
        {
            null => NullValueRepresentation,
            string s => s == "" ? "<empty>" : (s.Trim() == "" ? $"'{s}'" : s),
            _ => input
        };
    }
}