using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners.ScenarioScanners
{
    public interface IScenarioScanner
    {
        Scenario Scan(object testObject);
    }
}