using System;
using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class MethodNameScanner : DefaultScannerBase
    {
        private readonly MethodNameMatcher[] _matchers;

        public MethodNameScanner(params MethodNameMatcher[] matchers)
        {
            _matchers = matchers;
        }

        public MethodNameScanner()
            : this(
                    new[]{
                            new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true),
                            new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true)
                        })
        {
        }

        protected override IEnumerable<ExecutionStep> ScanForSteps()
        {
            var methods = TestObject.GetType().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var matcher in _matchers)
            {
                foreach (var method in methods)
                {
                    if (matcher.IsMethodOfInterest(method.Name))
                    {
                        var argAttributes = (WithArgsAttribute[])method.GetCustomAttributes(typeof(WithArgsAttribute), false);
                        object[] inputs = null;
                        if (argAttributes != null && argAttributes.Length > 0)
                            inputs = argAttributes[0].InputArguments;

                        // creating the method itself
                        yield return new ExecutionStep(method, inputs, NetToString.FromName(method.Name), matcher.Asserts, matcher.ShouldReport);

                        if (argAttributes != null && argAttributes.Length > 1)
                        {
                            for (int index = 1; index < argAttributes.Length; index++)
                            {
                                var argAttribute = argAttributes[index];
                                inputs = argAttribute.InputArguments;
                                if (inputs != null && inputs.Length > 0)
                                    yield return new ExecutionStep(method, inputs, NetToString.FromName(method.Name), matcher.Asserts, matcher.ShouldReport);
                            }
                        }

                        break;
                    }
                }
            }

            yield break;
        }
    }
}