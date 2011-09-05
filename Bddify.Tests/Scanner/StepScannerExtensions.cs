using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bddify.Core;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.ScenarioScanners;

namespace Bddify.Tests.Scanner
{
    internal static class StepScannerExtensions
    {
        internal static IList<ExecutionStep> Scan(this IScanForSteps scanner, object testObject)
        {
            // ToDo: this is rather hacky and is not DRY. Should think of a way to get rid of this
            return ScanForScenario.GetMethodsOfInterest(testObject.GetType()).SelectMany(m => scanner.Scan(m)).OrderBy(s => s.ExecutionOrder).ToList();
        }
    }
}
