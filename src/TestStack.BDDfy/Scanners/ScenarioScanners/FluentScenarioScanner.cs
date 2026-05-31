using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Scanners.ScenarioScanners;

internal class FluentScenarioScanner(List<Step> steps, string? title): IScenarioScanner
{
    public IEnumerable<Scenario> Scan(ITestContext testContext)
    {
        var scenarioText = title ?? testContext.TestObject.GetType().Name;
        if (testContext.Examples != null)
        {
            var scenarioId = Configurator.IdGenerator.GetScenarioId();
            return testContext.Examples.Select(example =>
                new Scenario(scenarioId, testContext.TestObject, CloneSteps(steps), scenarioText, example, testContext.Tags));
        }

        return [new Scenario(testContext.TestObject, steps, scenarioText, testContext.Tags)];
    }

    private static List<Step> CloneSteps(IEnumerable<Step> steps) => [.. steps.Select(static step => new Step(step))];
}
