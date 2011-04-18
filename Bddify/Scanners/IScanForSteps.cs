using System;
using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Scanners
{
    public interface IScanForSteps
    {
        IEnumerable<ExecutionStep> Scan(Type scenarioType);
    }
}