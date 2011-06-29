using Bddify.Core;

namespace Bddify.Scanners
{
    public interface IScanForScenarios
    {
        Scenario Scan(object testObject);
    }
}