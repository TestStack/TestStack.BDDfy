using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public interface IScanForSteps
    {
        IEnumerable<ExecutionStep> Scan(Type scenarioType);
        bool Handled { get; }
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
    }
}