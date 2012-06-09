using System;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners.StepScanners.MethodName
{
    public class DefaultMethodNameStepScanner : MethodNameStepScanner
    {
        public DefaultMethodNameStepScanner()
            : base(
                CleanupTheStepText,
                new[]
                {
                    new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                    new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                    new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveSetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("And_Given_", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveSetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.Transition, true),
                    new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveTransition, true),
                    new MethodNameMatcher(s => s.StartsWith("And_When_", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveTransition, true),
                    new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.Assertion, true),
                    new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.ConsecutiveAssertion, true),
                    new MethodNameMatcher(s => s.StartsWith("TearDown", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.TearDown, false),
                })
        {
        }

        static string CleanupTheStepText(string stepText)
        {
            if (stepText.StartsWith("and given ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "given ".Length);

            if (stepText.StartsWith("and when ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "when ".Length);

            if (stepText.StartsWith("AndGiven ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "given".Length);

            if (stepText.StartsWith("AndWhen ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "when".Length);

            return stepText;
        }
    }
}