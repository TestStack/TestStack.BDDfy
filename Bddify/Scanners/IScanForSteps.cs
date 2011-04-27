using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public interface IScanForSteps
    {
        int Priority { get; }
        IEnumerable<ExecutionStep> Scan(Type scenarioType);
    }

    public static class StepScanner
    {
        public static IEnumerable<MethodInfo> GetMethodsOfInterest(this Type scenarioType)
        {
            return scenarioType                
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.GetCustomAttributes(typeof(IgnoreStepAttribute), false).Any())
                .ToList();
        }

        public static string[] FlattenArrays(this object[] inputs)
        {
            var stringOffArray = new List<string>();
            foreach (var input in inputs)
            {
                var inputArray = input as Array;
                if(inputArray != null)
                {
                    var temp = (from object arrElement in inputArray select arrElement.ToString()).ToList();
                    stringOffArray.Add(string.Join(", ", temp));
                }
                else
                    stringOffArray.Add(input.ToString());
            }

            return stringOffArray.ToArray();
        }
    }
}