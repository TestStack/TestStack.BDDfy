using System;
using System.Linq;

namespace TestStack.BDDfy
{
    internal static class StepScannerExtensions
    {
        internal static object[] FlattenArrays(this object[] inputs)
        {
            return inputs.Select(FlattenArrays).ToArray();
        }

        public static object FlattenArrays(this object input)
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