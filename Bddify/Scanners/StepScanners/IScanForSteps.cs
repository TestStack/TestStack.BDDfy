using System.Collections.Generic;
using Bddify.Core;
using System.Reflection;

namespace Bddify.Scanners.StepScanners
{
    public interface IScanForSteps
    {
        IEnumerable<ExecutionStep> Scan(MethodInfo candidateMethod);
    }
}