# Configuration

BDDfy's behavior is controlled through the static `Configurator` class. Everything from step scanning to reporting is pluggable.

## The Configurator Class

```csharp
using TestStack.BDDfy.Configuration;

// All configuration is done via static properties:
Configurator.Processors       // Per-scenario pipeline
Configurator.BatchProcessors  // End-of-run reporters
Configurator.Scanners         // Step discovery
Configurator.IdGenerator      // Unique key generation
Configurator.StepExecutor     // Step execution strategy
Configurator.Humanizer        // Method name → readable text
Configurator.FluentScannerFactory  // Fluent scanner creation
Configurator.StepTitleFactory      // Step title generation
Configurator.CultureInfo      // Culture for formatting
Configurator.AsyncVoidSupportEnabled  // async void step support (default: true)
```

## Per-Scenario Processors

Processors run for each scenario in order of their `ProcessType`:

```csharp
// Disable console output
Configurator.Processors.ConsoleReport.Disable();

// Add a custom processor
Configurator.Processors.Add(() => new MyCustomProcessor());
```

Built-in processors (in execution order):
1. **TestRunner** — executes the scenario steps
2. **ConsoleReporter** — prints to console
3. **ExceptionProcessor** — handles/rethrows exceptions
4. **StoryCache** — stores results for batch reporters
5. **Disposer** — disposes the test object if `IDisposable`

### Enabling/Disabling

```csharp
Configurator.Processors.TestRunner.Disable();
Configurator.Processors.ConsoleReport.Disable();
Configurator.Processors.StoryCache.Disable();
```

## Batch Processors

Run once after all scenarios complete (typically reporters):

```csharp
Configurator.BatchProcessors.HtmlReport.Enable();
Configurator.BatchProcessors.HtmlMetroReport.Disable();
Configurator.BatchProcessors.MarkDownReport.Enable();
Configurator.BatchProcessors.DiagnosticsReport.Enable();

// Add custom batch processor
Configurator.BatchProcessors.Add(new MyBatchProcessor());
```

## Step Scanners

Control how BDDfy discovers steps in your test classes:

```csharp
// Disable attribute-based scanning
Configurator.Scanners.ExecutableAttributeScanner.Disable();

// Disable method-name scanning
Configurator.Scanners.DefaultMethodNameStepScanner.Disable();

// Add a custom step scanner
Configurator.Scanners.Add(() => new MyCustomStepScanner());
```

## Story Metadata Scanner

Replace how story metadata is discovered:

```csharp
Configurator.Scanners.StoryMetadataScanner = () => new MyCustomMetadataScanner();
```

## Humanizer

The humanizer converts method names into readable titles:

```csharp
// Replace with a custom humanizer
Configurator.Humanizer = new MyHumanizer();

public class MyHumanizer : IHumanizer
{
    public string Humanize(string name)
    {
        // Custom logic to convert PascalCase/underscored names to text
        return name.Replace("_", " ");
    }
}
```

## Step Executor

Override how individual steps are executed:

```csharp
Configurator.StepExecutor = new MyStepExecutor();

public class MyStepExecutor : IStepExecutor
{
    public void Execute(Step step, object testObject)
    {
        // Add logging, timing, retry logic, etc.
        Console.WriteLine($"Executing: {step.Title}");
        step.Execute(testObject);
    }
}
```

## Step Title Factory

Customize how step titles are generated from expressions:

```csharp
Configurator.StepTitleFactory = new MyStepTitleFactory();
```

### IncludeInputsInStepTitle

Controls whether step arguments are appended to step titles (default: `true`):

```csharp
Configurator.StepTitleFactory.IncludeInputsInStepTitle = false;
```

### AddGherkinPrefixToSecondarySteps

When a step has an explicit title (via `[Given("...")]`, `[When("...")]`, `[Then("...")]`, or `[StepTitle("...")]`), BDDfy prepends the Gherkin keyword to the title by default. For consecutive steps in the same group, this means they get an "And" prefix. Set this to `false` to disable the "And" prefix on consecutive custom-titled steps:

```csharp
Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = true;  // default
```

**With prefix enabled (default):**

```
Scenario: With prefix enabled
    Given the user is logged in
      And the cart has items
      And the payment gateway is available
    When the user checks out
    Then the order is confirmed
      And a confirmation email is sent
```

```csharp
Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = false;
```

**With prefix disabled:**

```
Scenario: With prefix disabled
    Given the user is logged in
      the cart has items
      the payment gateway is available
    When the user checks out
    Then the order is confirmed
      a confirmation email is sent
```

> **Note:** Primary step keywords (Given/When/Then) are always applied regardless of this setting. Only the consecutive "And" prefix on custom-titled steps is affected.

## Culture

Set the culture used for formatting values in reports:

```csharp
Configurator.CultureInfo = new CultureInfo("en-US");
```

## Async Void Support

BDDfy can detect and await `async void` step methods. Disable if it causes issues:

```csharp
Configurator.AsyncVoidSupportEnabled = false;
```

## Configuration Timing

Configure BDDfy before any tests run. With xUnit, use a module initializer or assembly fixture:

```csharp
using System.Runtime.CompilerServices;
using TestStack.BDDfy.Configuration;

public static class BDDfySetup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Configurator.BatchProcessors.HtmlReport.Disable();
        Configurator.BatchProcessors.MarkDownReport.Enable();
    }
}
```

With NUnit, use `[SetUpFixture]`:

```csharp
[SetUpFixture]
public class BDDfySetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        Configurator.BatchProcessors.HtmlMetroReport.Enable();
    }
}
```
