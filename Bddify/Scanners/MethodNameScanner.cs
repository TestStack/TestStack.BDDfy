using System;
using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;
using System.Linq;

namespace Bddify.Scanners
{
    public class MethodNameScanner : DefaultScannerBase
    {
        private readonly MethodNameMatcher[] _matchers;

        public MethodNameScanner(MethodNameMatcher[] matchers)
        {
            _matchers = matchers;
        }

        protected override IEnumerable<ExecutionStep> ScanForSteps()
        {
            var methodsToScan = TestObject.GetType().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
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
                            inputs = argAttributes[0].InputArguments;
                            stepMethodName += " with args (" + string.Join(", ", inputs) + ")";
                        }

                        // creating the method itself
                        yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ShouldReport);

                        if (argAttributes != null && argAttributes.Length > 1)
                        {
                            for (int index = 1; index < argAttributes.Length; index++)
                            {
                                stepMethodName = methodName;
                                var argAttribute = argAttributes[index];
                                inputs = argAttribute.InputArguments;
                                if (inputs != null && inputs.Length > 0)
                                {
                                    stepMethodName += " with args (" + string.Join(", ", inputs) + ")";
                                    yield return new ExecutionStep(method, inputs, stepMethodName, matcher.Asserts, matcher.ShouldReport);
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