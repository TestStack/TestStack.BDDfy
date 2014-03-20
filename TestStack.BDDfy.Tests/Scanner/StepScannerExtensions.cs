using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Tests.Scanner
{
    internal static class StepScannerExtensions
    {
        internal static IEnumerable<Step> Scan(this IStepScanner scanner, object testObject)
        {
            // ToDo: this is rather hacky and is not DRY. Should think of a way to get rid of this
            return new ReflectiveScenarioScanner()
                .GetMethodsOfInterest(testObject.GetType())
                .SelectMany(x => scanner.Scan(testObject, x))
                .OrderBy(s => s.ExecutionOrder)
                .ToList();
        }
    }
}
