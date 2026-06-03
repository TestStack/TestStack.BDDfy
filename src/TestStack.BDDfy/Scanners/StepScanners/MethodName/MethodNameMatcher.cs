namespace TestStack.BDDfy
{
    public class MethodNameMatcher(Predicate<string> isMethodOfInterest, ExecutionOrder executionOrder, string stepPrefix = "")
    {
        public MethodNameMatcher(Predicate<string> isMethodOfInterest, bool asserts, ExecutionOrder executionOrder, bool shouldReport)
            : this(isMethodOfInterest, executionOrder)
        {
            Asserts = asserts;
            ShouldReport = shouldReport;
        }

        public Predicate<string> IsMethodOfInterest { get; private set; } = isMethodOfInterest;
        public bool Asserts { get; set; } = false;
        public bool ShouldReport { get; set; } = true;
        public ExecutionOrder ExecutionOrder { get; private set; } = executionOrder;
        public string StepPrefix { get; set; } = stepPrefix;
    }
}