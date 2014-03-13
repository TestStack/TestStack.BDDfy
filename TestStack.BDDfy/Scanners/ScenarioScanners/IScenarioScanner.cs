using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
{
    public interface IScenarioScanner
    {
        Scenario Scan(object testObject);
    }
}