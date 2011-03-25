using System;

namespace Bddify.Scanners
{
    public class MethodNameMatcher
    {
        public MethodNameMatcher(Predicate<string> isMethodOfInterest, bool asserts, bool shouldReport = true)
        {
            IsMethodOfInterest = isMethodOfInterest;
            Asserts = asserts;
            ShouldReport = shouldReport;
        }

        public Predicate<string> IsMethodOfInterest { get; private set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
    }
}