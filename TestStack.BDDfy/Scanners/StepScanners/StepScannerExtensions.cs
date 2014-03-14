using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    internal static class StepScannerExtensions
    {
        internal static object[] FlattenArrays(this object[] inputs)
        {
            var flatArray = new List<object>();
            foreach (var input in inputs)
            {
                var inputArray = input as Array;
                if (inputArray != null)
                {
                    var temp = (from object arrElement in inputArray select GetSafeString(arrElement)).ToArray();
                    flatArray.Add(string.Join(", ", temp));
                }
                else if (input == null)
                    flatArray.Add("'null'");
                else
                    flatArray.Add(input);
            }

            return flatArray.ToArray();
        }

        static string GetSafeString(object input)
        {
            if (input == null)
                return "'null'";

            return input.ToString();
        }
    }
}