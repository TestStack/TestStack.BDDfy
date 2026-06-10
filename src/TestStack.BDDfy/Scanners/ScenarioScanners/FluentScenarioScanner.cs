using System.Reflection;
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
            var example = parameterizedInfo.Example ?? BuildExampleFromStepArguments(parameterizedInfo.Method.GetParameters(), steps);

            // Set the example table on the context so step titles use placeholder formatting (e.g. <endpoint>)
            if (example is not null)
            {
                var table = new ExampleTable([.. parameterizedInfo.Method.GetParameters().Select(p => p.Name!)])
                {
                    example
                };

                testContext.Examples = table;
            }

            return [new Scenario(parameterizedInfo.StableScenarioId, testContext.TestObject, steps, scenarioText, example, testContext.Tags)];
        }

        return [new Scenario(testContext.TestObject, steps, scenarioText, testContext.Tags)];
    }

    private static Example? BuildExampleFromStepArguments(ParameterInfo[] parameters, List<Step> steps)
    {
        var allArgs = steps.SelectMany(s => s.Arguments).ToArray();
        var values = new List<ExampleValue>();
        int rowIndex = 0;

        foreach (var param in parameters)
        {
            var paramName = param.Name!;
            var matchingArg = allArgs.FirstOrDefault(a =>
                string.Equals(a.Name, paramName, StringComparison.OrdinalIgnoreCase));

            var value = matchingArg?.Value;
            values.Add(new ExampleValue(paramName, value, () => rowIndex));
        }

        return values.Count > 0 ? new Example([.. values]) : null;
    }

    private static List<Step> CloneSteps(IEnumerable<Step> steps) => [.. steps.Select(static step => new Step(step))];
}
