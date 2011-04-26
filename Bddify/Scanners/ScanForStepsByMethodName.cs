using System;
using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;
using System.Linq;

namespace Bddify.Scanners
{
    public class ScanForStepsByMethodName : IScanForSteps
    {
        private readonly MethodNameMatcher[] _matchers;

        public ScanForStepsByMethodName(MethodNameMatcher[] matchers)
        {
            _matchers = matchers;
        }

        public int Priority
        {
            get { return 2; }
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
                    var methodName = NetToString.FromName(method.Name);

                    if (matcher.IsMethodOfInterest(method.Name))
                    {
                        foundMethods.Add(method);

                        var argAttributes = (RunStepWithArgsAttribute[])method.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                        object[] inputs = null;
                        var stepMethodName  = methodName;

                        if (argAttributes != null && argAttributes.Length > 0)
                        {
                            var runStepWithArgsAttribute = argAttributes[0];
                            inputs = runStepWithArgsAttribute.InputArguments;
                            if (string.IsNullOrEmpty(runStepWithArgsAttribute.StepTextTemplate))
                                stepMethodName += " " + string.Join(", ", inputs);
                            else
                                stepMethodName = string.Format(runStepWithArgsAttribute.StepTextTemplate, runStepWithArgsAttribute.InputArguments);
                        }

                        // creating the method itself
                        yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);

                        if (argAttributes != null && argAttributes.Length > 1)
                        {
                            for (int index = 1; index < argAttributes.Length; index++)
                            {
                                stepMethodName = methodName;
                                var argAttribute = argAttributes[index];
                                inputs = argAttribute.InputArguments;
                                if (inputs != null && inputs.Length > 0)
                                {
                                    if (string.IsNullOrEmpty(argAttribute.StepTextTemplate))
                                        stepMethodName += " " + string.Join(", ", inputs);
                                    else
                                        stepMethodName = string.Format(argAttribute.StepTextTemplate, argAttribute.InputArguments);

                                    yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);
                                }
                            }
                        }
                    }
                }
            }

            yield break;
        }
    }
}