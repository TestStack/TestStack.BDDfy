using Bddify.Core;

namespace Bddify.Scanners.ScenarioScanners
{
    public interface IScenarioScanner
    {
        Scenario Scan(object testObject);
    }
}