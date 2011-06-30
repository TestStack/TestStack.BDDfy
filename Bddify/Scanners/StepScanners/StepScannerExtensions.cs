using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify.Scanners.StepScanners
{
    internal static class StepScannerExtensions
    {
        internal static IEnumerable<MethodInfo> GetMethodsOfInterest(this Type scenarioType)
        {
            return scenarioType                
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.GetCustomAttributes(typeof(IgnoreStepAttribute), false).Any())
                .ToList();
        }

        internal static string[] FlattenArrays(this object[] inputs)
        {
            var stringOffArray = new List<string>();
            foreach (var input in inputs)
            {
                var inputArray = input as Array;
                if (inputArray != null)
                {
                    var temp = (from object arrElement in inputArray select GetSafeString(arrElement)).ToArray();
                    stringOffArray.Add(string.Join(", ", temp));
                }
                else
                    stringOffArray.Add(GetSafeString(input));
            }

            return stringOffArray.ToArray();
        }

        static string GetSafeString(object input)
        {
            if (input == null)
                return "'null'";

            return input.ToString();
        }
    }
}