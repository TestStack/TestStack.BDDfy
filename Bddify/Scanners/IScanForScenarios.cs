using System;
using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Scanners
{
    public interface IScanForScenarios
    {
        IEnumerable<Scenario> Scan(Type scenarioType);
    }
}