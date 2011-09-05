using Bddify.Core;

namespace Bddify.Scanners.ScenarioScanners
{
    public interface IScanForScenario
    {
        Scenario Scan(object testObject);
    }
}