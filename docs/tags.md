# Tags

Tags let you categorize scenarios for filtering and organization in reports.

## Adding Tags

Use the `WithTags()` extension method before calling `BDDfy()`:

```csharp
[Fact]
public void Execute()
{
    this.WithTags("smoke", "critical")
        .BDDfy();
}
```

### With the Fluent API

Tags are applied on the test object before the fluent chain:

```csharp
[Fact]
public void Execute()
{
    this.WithTags("integration")
        .Given(s => s.Setup())
        .When(s => s.Action())
        .Then(s => s.Verify())
        .BDDfy();
}
```

## Tags in Reports

### Text/Console Report

Tags appear at the end of the scenario:

```
Scenario: User logs in
    Given the user exists
    When credentials are submitted
    Then access is granted

Tags: smoke, critical
```

### HTML Reports

Tags are displayed as badges on each scenario, allowing visual categorization.

### Markdown Reports

Tags are included in the scenario output for searchability.

## Use Cases

- **Smoke tests**: Tag critical-path scenarios with `"smoke"` for quick CI runs
- **Categories**: Group by feature area (`"auth"`, `"payments"`, `"admin"`)
- **Priority**: Mark tests as `"P1"`, `"P2"`, etc.
- **Integration markers**: Tag tests requiring external dependencies with `"integration"`

## Filtering by Tags

While BDDfy itself doesn't filter test execution by tags (that's your test runner's job), you can use tags in custom reporters to filter report output:

```csharp
public class TagFilteredReporter : IBatchProcessor
{
    private readonly string _requiredTag;

    public TagFilteredReporter(string requiredTag)
    {
        _requiredTag = requiredTag;
    }

    public void Process(IEnumerable<Story> stories)
    {
        var filtered = stories.Select(s => new
        {
            Story = s,
            Scenarios = s.Scenarios
                .Where(sc => sc.Tags.Contains(_requiredTag))
                .ToList()
        }).Where(x => x.Scenarios.Count > 0);

        // Generate report from filtered scenarios...
    }
}
```
