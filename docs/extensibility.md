# Extensibility

BDDfy's core is a thin pipeline that delegates all work to pluggable components. You can replace or extend virtually any part of the framework.

## Custom Reporter (IProcessor)

A per-scenario reporter receives each `Story` as it completes:

```csharp
public class CustomTextReporter : IProcessor
{
    public ProcessType ProcessType => ProcessType.Report;

    public void Process(Story story)
    {
        foreach (var scenario in story.Scenarios)
        {
            Console.WriteLine($"  SCENARIO: {scenario.Title} [{scenario.Result}]");

            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
            {
                Console.WriteLine($"    [{step.Result}] {step.Title}");
            }
        }
    }
}
```

Register it:

```csharp
Configurator.Processors.Add(() => new CustomTextReporter());
```

### ProcessType Ordering

Processors execute in `ProcessType` order:

| ProcessType | Value | Purpose |
|-------------|-------|---------|
| `Execute` | 0 | Run steps |
| `Report` | 1 | Generate output |
| `HandleExceptions` | 2 | Throw/swallow exceptions |
| `Cache` | 3 | Store for batch processing |
| `Dispose` | 4 | Cleanup |

## Custom Batch Reporter (IBatchProcessor)

Batch reporters run once after all scenarios complete, receiving all stories:

```csharp
public class JsonReporter : IBatchProcessor
{
    public void Process(IEnumerable<Story> stories)
    {
        var data = stories.Select(s => new
        {
            s.Metadata?.Title,
            Scenarios = s.Scenarios.Select(sc => new
            {
                sc.Title,
                Result = sc.Result.ToString(),
                Steps = sc.Steps.Select(st => new
                {
                    st.Title,
                    Result = st.Result.ToString(),
                    Duration = st.Duration.TotalMilliseconds
                })
            })
        });

        var json = System.Text.Json.JsonSerializer.Serialize(data, 
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("BDDfy-results.json", json);
    }
}

// Register
Configurator.BatchProcessors.Add(new JsonReporter());
```

## Custom Step Scanner (IStepScanner)

Step scanners discover steps from a test object. Implement `IStepScanner` to add your own discovery logic:

```csharp
public class AttributeOrderedStepScanner : IStepScanner
{
    public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo candidateMethod)
    {
        // Only handle methods with your custom attribute
        var attr = candidateMethod.GetCustomAttribute<MyStepAttribute>();
        if (attr == null)
            yield break;

        var action = StepActionFactory.GetStepAction(candidateMethod, testContext.TestObject);

        yield return new Step(action)
        {
            Title = new StepTitle(attr.Title ?? candidateMethod.Name),
            ExecutionOrder = attr.Order switch
            {
                "Given" => ExecutionOrder.SetupState,
                "When" => ExecutionOrder.Transition,
                "Then" => ExecutionOrder.Assertion,
                _ => ExecutionOrder.Assertion
            },
            ShouldReport = true
        };
    }

    public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo candidateMethod, Example example)
    {
        // Handle example-based scenarios if needed
        return Scan(testContext, candidateMethod);
    }
}

// Register
Configurator.Scanners.Add(() => new AttributeOrderedStepScanner());
```

## Custom Step Executor (IStepExecutor)

The step executor controls how each step is invoked. Use it for cross-cutting concerns:

### Retry Logic

```csharp
public class RetryStepExecutor : IStepExecutor
{
    private readonly int _maxRetries;

    public RetryStepExecutor(int maxRetries = 3)
    {
        _maxRetries = maxRetries;
    }

    public void Execute(Step step, object testObject)
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                step.Execute(testObject);
                return;
            }
            catch when (attempt < _maxRetries)
            {
                Thread.Sleep(100 * attempt);
            }
        }
    }
}

Configurator.StepExecutor = new RetryStepExecutor(maxRetries: 3);
```

### Logging / Timing

```csharp
public class TimingStepExecutor : IStepExecutor
{
    public void Execute(Step step, object testObject)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            step.Execute(testObject);
        }
        finally
        {
            sw.Stop();
            Console.WriteLine($"  [{sw.ElapsedMilliseconds}ms] {step.Title}");
        }
    }
}

Configurator.StepExecutor = new TimingStepExecutor();
```

## Custom Humanizer (IHumanizer)

Replace the method-name-to-title conversion logic:

```csharp
public class UpperCaseHumanizer : IHumanizer
{
    public string Humanize(string name)
    {
        // Split PascalCase, join with spaces, uppercase
        var words = System.Text.RegularExpressions.Regex.Replace(
            name, "([A-Z])", " $1").Trim();
        return words.ToUpperInvariant();
    }
}

Configurator.Humanizer = new UpperCaseHumanizer();
```

## Custom Story Metadata Scanner

Replace how BDDfy finds story information on classes:

```csharp
public class XmlStoryMetadataScanner : IStoryMetadataScanner
{
    public StoryMetadata? Scan(object testObject, Type explicitStoryType = null)
    {
        var type = explicitStoryType ?? testObject.GetType();

        // Load story metadata from an XML file, database, etc.
        var storyData = LoadFromXml(type.Name);
        if (storyData == null) return null;

        return new StoryMetadata(
            type,
            narrative1: storyData.AsA,
            narrative2: storyData.IWant,
            narrative3: storyData.SoThat,
            title: storyData.Title);
    }
}

Configurator.Scanners.StoryMetadataScanner = () => new XmlStoryMetadataScanner();
```

## Custom Fluent Scanner Factory

Replace how the fluent API creates its internal scanner:

```csharp
public class MyFluentScannerFactory : IFluentScannerFactory
{
    public IFluentScanner Create<TScenario>(TScenario testObject) where TScenario : class
    {
        return new FluentScanner<TScenario>(testObject);
    }
}

Configurator.FluentScannerFactory = new MyFluentScannerFactory();
```

## Combining Extensions

You can combine multiple extensions. For example, use a custom step executor with a custom reporter:

```csharp
[ModuleInitializer]
public static void Setup()
{
    // Timing executor
    Configurator.StepExecutor = new TimingStepExecutor();

    // Custom JSON batch reporter
    Configurator.BatchProcessors.Add(new JsonReporter());

    // Disable default HTML, enable Markdown
    Configurator.BatchProcessors.HtmlReport.Disable();
    Configurator.BatchProcessors.MarkDownReport.Enable();
}
```
