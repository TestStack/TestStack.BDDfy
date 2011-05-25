using System;
using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;
using System.Linq;

namespace Bddify.Scanners
{
    public class MethodNameStepScanner : IScanForSteps
    {
        private readonly MethodNameMatcher[] _matchers;

        public MethodNameStepScanner(MethodNameMatcher[] matchers)
        {
            _matchers = matchers;
        }

        public int Priority
        {
            get { return 20; }
        }

        public IEnumerable<ExecutionStep> Scan(Type scenarioType)
        {
            var methodsToScan = scenarioType.GetMethodsOfInterest();
            var foundMethods = new List<MethodInfo>();

            foreach (var matcher in _matchers)
            {
                // if a method is already matched we should exclude it because it may match against another criteria too
                // e.g. a method starting with AndGiven matches against both AndGiven and And
                foreach (var method in methodsToScan.Except(foundMethods))
                {
                    var methodName = CleanupTheStepText(NetToString.Convert(method.Name));

                    if (!matcher.IsMethodOfInterest(method.Name)) 
                        continue;

                    foundMethods.Add(method);

                    var argAttributes = (RunStepWithArgsAttribute[])method.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                    object[] inputs = null;
                    var stepMethodName = methodName;

                    if (argAttributes == null || argAttributes.Length == 0)
                    {
                        // creating the method itself
                        yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);
                        continue;
                    }

                    foreach (var argAttribute in argAttributes)
                    {
                        inputs = argAttribute.InputArguments;
                        if (inputs != null && inputs.Length > 0)
                        {
                            if (string.IsNullOrEmpty(argAttribute.StepTextTemplate))
                                stepMethodName = methodName + " " + string.Join(", ", inputs.FlattenArrays());
                            else
                                stepMethodName = string.Format(argAttribute.StepTextTemplate, inputs.FlattenArrays());

                            yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);
                        }
                    }
                }
            }

            yield break;
        }

        static string CleanupTheStepText(string stepText)
        {
            // ToDo: replace 'and given' and 'and when' with 'and'
            //if (stepText.StartsWith("and given ", StringComparison.OrdinalIgnoreCase) ||
            //    stepText.StartsWith("and when ", StringComparison.OrdinalIgnoreCase))
            //    return "and ";

            return stepText;
        }
    }
}