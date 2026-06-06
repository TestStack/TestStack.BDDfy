using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Scanners.ScenarioScanners;

internal class FluentScenarioScanner(List<Step> steps, string? title): IScenarioScanner
{
    public IEnumerable<Scenario> Scan(ITestContext testContext)
    {
        var scenarioText = title ?? testContext.TestObject.GetType().Name;
        if (testContext.Examples is not null)
        {
            var scenarioId = Configurator.IdGenerator.GetScenarioId();
            return testContext.Examples.Select(example =>
                new Scenario(scenarioId, testContext.TestObject, CloneSteps(steps), scenarioText, example, testContext.Tags));
        }

        // Check if this is a parameterized test (e.g. [InlineData], [TestCase])
        var parameterizedInfo = ParameterizedTestDetector.Detect(testContext.TestObject);
        if (parameterizedInfo is not null)
        {
            return [new Scenario(parameterizedInfo.StableScenarioId, testContext.TestObject, steps, scenarioText, parameterizedInfo.Example, testContext.Tags)];
        }

        return [new Scenario(testContext.TestObject, steps, scenarioText, testContext.Tags)];
    }

    private static List<Step> CloneSteps(IEnumerable<Step> steps) => [.. steps.Select(static step => new Step(step))];
}
