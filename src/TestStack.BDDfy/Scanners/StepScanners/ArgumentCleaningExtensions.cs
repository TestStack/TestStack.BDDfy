using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    internal static class ArgumentCleaningExtensions
    {
        internal static object[] FlattenArrays(this IEnumerable<object> inputs)
        {
            return inputs.Select(FlattenArray).ToArray();
        }

        public static object FlattenArray(this object input)
        {
            var inputArray = input as Array;
            if (inputArray != null)
            {
                var temp = (from object arrElement in inputArray select GetSafeString(arrElement)).ToArray();
                return string.Join(", ", temp);
            }

            if (input == null) return "'null'";

            return input;
        }

        static string GetSafeString(object input)
        {
            if (input == null)
                return "'null'";

            return input.ToString();
        }
    }
}