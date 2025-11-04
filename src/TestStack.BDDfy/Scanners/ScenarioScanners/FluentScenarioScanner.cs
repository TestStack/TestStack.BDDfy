using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Scanners.ScenarioScanners;

internal class FluentScenarioScanner(List<Step> steps, string title): IScenarioScanner
{
    private readonly string _title = title;
    private readonly List<Step> _steps = steps;

    public IEnumerable<Scenario> Scan(ITestContext testContext)
    {
        var scenarioText = _title ?? testContext.TestObject.GetType().Name;
        if (testContext.Examples != null)
        {
            var scenarioId = Configurator.IdGenerator.GetScenarioId();
            return testContext.Examples.Select(example =>
                new Scenario(scenarioId, testContext.TestObject, CloneSteps(_steps), scenarioText, example, testContext.Tags));
        }

        return [new Scenario(testContext.TestObject, _steps, scenarioText, testContext.Tags)];
    }

    private static List<Step> CloneSteps(IEnumerable<Step> steps) => [.. steps.Select(static step => new Step(step))];
}
