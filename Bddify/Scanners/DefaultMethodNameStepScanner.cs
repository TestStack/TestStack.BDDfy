using System;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class DefaultMethodNameStepScanner : MethodNameStepScanner
    {
        public DefaultMethodNameStepScanner()
            : base(new[]
            {
                new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true),
                new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveSetupState, true),
                new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.Transition, true),
                new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveTransition, true),
                new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.Assertion, true),
                new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.ConsecutiveAssertion, true),
                new MethodNameMatcher(s => s.StartsWith("TearDown", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.TearDown, false),
            })
        {
        }
    }
}