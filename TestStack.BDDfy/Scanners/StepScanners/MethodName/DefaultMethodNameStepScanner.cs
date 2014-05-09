using System;

namespace TestStack.BDDfy
{
    public class DefaultMethodNameStepScanner : MethodNameStepScanner
    {
        public DefaultMethodNameStepScanner()
            : base(CleanupTheStepText)
        {
            AddMatcher(new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), ExecutionOrder.Initialize) { ShouldReport = false });
            AddMatcher(new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), ExecutionOrder.Initialize) { ShouldReport = false });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), ExecutionOrder.SetupState));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveSetupState));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("And_Given_", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveSetupState));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), ExecutionOrder.Transition));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveTransition));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("And_When_", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveTransition));
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), ExecutionOrder.Assertion) { Asserts = true });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveAssertion) { Asserts = true });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("AndThen", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveAssertion) { Asserts = true });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("And_Then_", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveAssertion) { Asserts = true });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("But", StringComparison.OrdinalIgnoreCase), ExecutionOrder.ConsecutiveAssertion) { Asserts = true });
            AddMatcher(new MethodNameMatcher(s => s.StartsWith("TearDown", StringComparison.OrdinalIgnoreCase), ExecutionOrder.TearDown) { ShouldReport = false });
        }

        static string CleanupTheStepText(string stepText)
        {
            if (stepText.StartsWith("and given ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "given ".Length);

            if (stepText.StartsWith("and when ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "when ".Length);

            if (stepText.StartsWith("and then ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "then ".Length);

            if (stepText.StartsWith("AndGiven ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "given".Length);

            if (stepText.StartsWith("AndWhen ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "when".Length);

            if (stepText.StartsWith("AndThen ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "then".Length);

            return stepText;
        }
    }
}