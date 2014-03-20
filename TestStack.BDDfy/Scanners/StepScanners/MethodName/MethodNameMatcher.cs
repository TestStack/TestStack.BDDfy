using System;

namespace TestStack.BDDfy
{
    public class MethodNameMatcher
    {
        public MethodNameMatcher(Predicate<string> isMethodOfInterest, bool asserts, ExecutionOrder executionOrder, bool shouldReport)
        {
            IsMethodOfInterest = isMethodOfInterest;
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
        }

        public Predicate<string> IsMethodOfInterest { get; private set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public ExecutionOrder ExecutionOrder { get; private set; }
    }
}