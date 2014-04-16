using System.Collections.Generic;

namespace TestStack.BDDfy
{
    public interface IScenarioScanner
    {
        IEnumerable<Scenario> Scan(object testObject);
    }
}