# Getting Started with BDDfy

BDDfy is the simplest BDD (Behavior-Driven Development) framework for .NET. It turns your test classes into living documentation with human-readable reports — no special test runners or DSL files required.

## Installation

Install via NuGet:

```shell
dotnet add package TestStack.BDDfy
```

Or via the Package Manager Console:

```shell
Install-Package TestStack.BDDfy
```

BDDfy works with any test framework — xUnit, NUnit, MSTest — or no framework at all.

## Your First Scenario

The fastest way to write a BDDfy test is using **method naming conventions**. Name your methods with Given/When/Then prefixes and call `this.BDDfy()`:

```csharp
using Xunit;
using TestStack.BDDfy;

public class ShouldBeAbleToReturnItem
{
    void GivenTheItemWasPurchasedRecently()
    {
        // arrange
    }

    void WhenTheCustomerReturnsTheItem()
    {
        // act
    }

    void ThenTheItemShouldBeAccepted()
    {
        // assert
    }

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

When you run this test, BDDfy automatically:
1. Discovers steps by scanning method names for Given/When/Then prefixes
2. Executes them in the correct order
3. Produces a console report and an HTML report

Console output:

```
Scenario: Should be able to return item
    Given the item was purchased recently
    When the customer returns the item
    Then the item should be accepted
```

## How Step Discovery Works

BDDfy converts your method names into readable step titles using a humanizer. It supports:

- **PascalCase**: `GivenTheCardIsValid` → "Given the card is valid"
- **Underscored**: `Given_the_card_is_valid` → "Given the card is valid"
- **camelCase**: `givenTheCardIsValid` → "Given the card is valid"

Supported prefixes: `Given`, `AndGiven`, `When`, `AndWhen`, `Then`, `AndThen`, `But`.

## Two APIs

BDDfy offers two main approaches to defining scenarios:

### Reflective API (Convention-based)

Steps are discovered automatically from method names or `[Given]`/`[When]`/`[Then]` attributes. Best for simple, self-contained scenarios.

```csharp
[Fact]
public void Execute()
{
    this.BDDfy();
}
```

→ [Full Reflective API documentation](reflective-api.md)

### Fluent API (Explicit)

Steps are declared using a fluent builder with lambda expressions. Best when you need explicit control over step ordering, titles, or reuse.

```csharp
[Fact]
public void Execute()
{
    this.Given(s => s.TheCardIsDisabled())
        .When(s => s.TheAccountHolderRequestsCash(20))
        .Then(s => s.TheAtmRetainsTheCard())
        .BDDfy();
}
```

→ [Full Fluent API documentation](fluent-api.md)

## Reports

By default, BDDfy generates:

- **Console report** — printed to standard output after each scenario
- **HTML report** — a standalone HTML file written to your output directory after all tests complete

→ [Full Reporters documentation](reporters.md)

## Adding a Story

You can group scenarios under a user story using the `[Story]` attribute:

```csharp
[Story(
    AsA = "As an account holder",
    IWant = "I want to withdraw cash from an ATM",
    SoThat = "So that I can get money when the bank is closed")]
public class AccountHasInsufficientFund
{
    // steps...

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

→ [Full Stories documentation](stories.md)

## Standalone Scenarios (No Story)

You don't need a story. If you omit the `[Story]` attribute, BDDfy uses the namespace as a grouping mechanism in reports:

```csharp
public class CanWorkWithoutAStory
{
    void Given_no_story_is_provided() { }
    void When_we_BDDfy_the_class() { }
    void Then_the_namespace_is_used_in_the_report() { }

    [Fact]
    public void RunTestWithoutAStory()
    {
        this.BDDfy();
    }
}
```

## Next Steps

| Topic | Description |
|-------|-------------|
| [Reflective API](reflective-api.md) | Method naming conventions and executable attributes |
| [Fluent API](fluent-api.md) | Fluent Given/When/Then builder |
| [Stories](stories.md) | Story metadata and grouping |
| [Examples](examples.md) | Data-driven scenarios with ExampleTable |
| [Reporters](reporters.md) | Console, HTML, Markdown, Text, and Diagnostics reporters |
| [Configuration](configuration.md) | Customizing the BDDfy pipeline |
| [Extensibility](extensibility.md) | Custom reporters, scanners, and step executors |
| [Async Support](async-support.md) | Async Task and async void steps |
| [Tags](tags.md) | Tagging and filtering scenarios |
