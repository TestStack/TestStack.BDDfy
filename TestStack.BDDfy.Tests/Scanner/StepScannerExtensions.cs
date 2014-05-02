using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Tests.Scanner
{
    internal static class StepScannerExtensions
    {
        internal static IEnumerable<Step> Scan(this IStepScanner scanner, ITestContext testContext)
        {
            // ToDo: this is rather hacky and is not DRY. Should think of a way to get rid of this
            return new ReflectiveScenarioScanner()
                .GetMethodsOfInterest(testContext.TestObject.GetType())
                .SelectMany(x => scanner.Scan(testContext, x))
                .OrderBy(s => s.ExecutionOrder)
                .ToList();
        }
    }
}
