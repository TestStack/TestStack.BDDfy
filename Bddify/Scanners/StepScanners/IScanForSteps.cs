using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners
{
    public interface IScanForSteps
    {
        int Priority { get; }
        IEnumerable<ExecutionStep> Scan(object testObject);
    }
}