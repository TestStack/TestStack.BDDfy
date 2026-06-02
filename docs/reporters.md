# Reporters

BDDfy generates human-readable reports from your test runs. Reports are produced at two levels:

- **Per-scenario processors** (`IProcessor`) — run after each scenario (e.g., console output)
- **Batch processors** (`IBatchProcessor`) — run once after all scenarios complete (e.g., HTML file)

## Console Reporter

Enabled by default. Prints each scenario to standard output immediately after it runs.

```
Story: Account holder withdraws cash
    As an account holder
    I want to withdraw cash from an ATM
    So that I can get money when the bank is closed

  Scenario: Account has insufficient fund
      Given the account balance is $10
        And the card is valid
      When the account holder requests $20
      Then the atm should not dispense any money        [Passed]
        And the atm should say there are insufficient funds [Passed]
```

### Disabling Console Output

```csharp
Configurator.Processors.ConsoleReport.Disable();
```

## HTML Reporter (Classic)

Enabled by default. Produces a standalone HTML file (`BDDfy.html`) in your output directory after all tests complete.

Features:
- Collapsible stories and scenarios
- Color-coded pass/fail/not-implemented status
- Duration display
- Example tables rendered inline

### Configuration

```csharp
Configurator.BatchProcessors.HtmlReport.Disable();

// Or customize:
Configurator.BatchProcessors.HtmlReport.Enable(
    () => new HtmlReporter(new CustomHtmlConfig()));
```

Implement `IHtmlReportConfiguration` to customize:

```csharp
public class CustomHtmlConfig : DefaultHtmlReportConfiguration
{
    public override string ReportHeader => "My Project";
    public override string ReportDescription => "Acceptance Tests";
    public override string OutputFileName => "AcceptanceTests.html";
    public override string OutputPath => @"C:\Reports";
    public override bool ResolveJqueryFromCdn => false;

    public override bool RunsOn(Story story)
    {
        // Filter which stories appear in this report
        return true;
    }
}
```

### Named HTML Reports

Use `htmlReportName` to generate multiple HTML files (one per name):

```csharp
this.BDDfy(htmlReportName: "ATM");
```

## HTML Reporter (Metro)

A modern flat-design alternative to the classic report. Disabled by default.

```csharp
// Enable Metro, disable Classic:
Configurator.BatchProcessors.HtmlReport.Disable();
Configurator.BatchProcessors.HtmlMetroReport.Enable();
```

## Markdown Reporter

Produces a `BDDfy.md` file. Disabled by default.

```csharp
Configurator.BatchProcessors.MarkDownReport.Enable();
```

Example output:

```markdown
# Story: Account holder withdraws cash

As an account holder
I want to withdraw cash from an ATM
So that I can get money when the bank is closed

## Scenario: Account has insufficient fund

Given the account balance is $10
  And the card is valid
When the account holder requests $20
Then the atm should not dispense any money
  And the atm should say there are insufficient funds
```

## Text Reporter

The `TextReporter` is the base class used by `ConsoleReporter`. It formats scenarios as indented plain text with step results and example tables. You can subclass it for custom text output (e.g., writing to a file or test output).

```csharp
public class FileTextReporter : TextReporter
{
    protected override void Write(string text, params object[] args)
        => File.AppendAllText("report.txt", string.Format(text, args));

    protected override void WriteLine(string? text = null)
        => File.AppendAllText("report.txt", text + Environment.NewLine);

    protected override void WriteLine(string text, params object[] args)
        => File.AppendAllText("report.txt", string.Format(text, args) + Environment.NewLine);
}
```

Register it:

```csharp
Configurator.Processors.Add(() => new FileTextReporter());
```

## Diagnostics Reporter (JSON)

Produces a `Diagnostics.json` file with structured data about all stories, scenarios, and steps. Disabled by default.

```csharp
Configurator.BatchProcessors.DiagnosticsReport.Enable();
```

Useful for:
- CI/CD pipeline integration
- Custom dashboard consumption
- Test result analysis tools

## Enabling / Disabling Reporters

All batch reporters use a factory pattern with `Enable()` and `Disable()` methods:

```csharp
// Disable all built-in batch reports
Configurator.BatchProcessors.HtmlReport.Disable();
Configurator.BatchProcessors.HtmlMetroReport.Disable();
Configurator.BatchProcessors.MarkDownReport.Disable();
Configurator.BatchProcessors.DiagnosticsReport.Disable();

// Enable only Markdown
Configurator.BatchProcessors.MarkDownReport.Enable();
```

## Adding a Custom Batch Reporter

Implement `IBatchProcessor`:

```csharp
public class MyCustomReporter : IBatchProcessor
{
    public void Process(IEnumerable<Story> stories)
    {
        // Generate your report from the story/scenario/step data
    }
}

// Register it
Configurator.BatchProcessors.Add(new MyCustomReporter());
```

## Report Filtering with RunsOn

The `IHtmlReportConfiguration.RunsOn(Story)` method lets you filter which stories appear in a report. Use this to split stories across multiple report files:

```csharp
public class AtmReportConfig : DefaultHtmlReportConfiguration
{
    public override string OutputFileName => "ATM.html";

    public override bool RunsOn(Story story)
    {
        return story.Metadata?.Type.Namespace?.Contains("Atm") == true;
    }
}
```
