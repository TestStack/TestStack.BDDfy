using System;

namespace TestStack.BDDfy
{
    public class MethodNameMatcher
    {
        public MethodNameMatcher(Predicate<string> isMethodOfInterest, bool asserts, ExecutionOrder executionOrder, bool shouldReport)
            : this(isMethodOfInterest, executionOrder)
        {
            Asserts = asserts;
            ShouldReport = shouldReport;
        }

        public MethodNameMatcher(Predicate<string> isMethodOfInterest, ExecutionOrder executionOrder)
        {
            IsMethodOfInterest = isMethodOfInterest;
            ExecutionOrder = executionOrder;
            Asserts = false;
            ShouldReport = true;
        }

        public Predicate<string> IsMethodOfInterest { get; private set; }
        public bool Asserts { get; set; }
        public bool ShouldReport { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }
    }
}