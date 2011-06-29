using Bddify.Core;

namespace Bddify.Scanners.ScenarioScanners
{
    public interface IScanForScenarios
    {
        Scenario Scan(object testObject);
    }
}