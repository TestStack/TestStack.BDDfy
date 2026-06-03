# Fluent API

The Fluent API gives you explicit control over step definitions using a chainable builder. Steps are declared with lambda expressions, making them refactoring-friendly and type-safe.

## Basic Usage

```csharp
using Xunit;
using TestStack.BDDfy;

public class CardHasBeenDisabled
{
    void GivenTheCardIsDisabled() { }
    void WhenTheAccountHolderRequests(int amount) { }
    void ThenTheAtmRetainsTheCard() { }
    void AndTheAtmDisplaysRetainedMessage() { }

    [Fact]
    public void Execute()
    {
        this.Given(s => s.GivenTheCardIsDisabled())
            .When(s => s.WhenTheAccountHolderRequests(20))
            .Then(s => s.ThenTheAtmRetainsTheCard())
                .And(s => s.AndTheAtmDisplaysRetainedMessage())
            .BDDfy();
    }
}
```

Report output:

```
Scenario: Card has been disabled
    Given the card is disabled
    When the account holder requests 20
    Then the atm retains the card
      And the atm displays retained message
```

## Step Title from Method Name

By default, BDDfy humanizes the method name referenced in the lambda expression. Method parameters are appended to the title automatically.

## Custom Step Text

### Using a template string

Provide a template as the second argument. Use `{0}`, `{1}`, etc. for parameter placeholders:

```csharp
this.Given(s => s.SetupBalance(100), "Given the account balance is ${0}")
    .When(s => s.Withdraw(20), "When the user withdraws ${0}")
    .Then(s => s.CheckBalance(80), "Then the balance should be ${0}")
    .BDDfy();
```

### Using a plain title string (no lambda)

You can provide just a title with no action — useful for placeholder steps with examples:

```csharp
this.Given("Given there are <start> cucumbers")
    .When(s => s.Eat())
    .Then(s => s.Verify())
    .BDDfy();
```

### Using an Action/Func delegate with a title

When you don't need the expression tree (no automatic title extraction):

```csharp
this.Given(() => Setup(), "Given the system is initialized")
    .When(() => DoWork(), "When work is performed")
    .Then(() => Assert.True(result), "Then the result is true")
    .BDDfy();
```

## Controlling Input Display

Pass `includeInputsInStepTitle: true` to append method arguments to the title, or `false` to suppress them:

```csharp
this.Given(s => s.AccountBalance(100), includeInputsInStepTitle: true)
    // Title: "Given account balance 100"
    .When(s => s.Withdraw(50), includeInputsInStepTitle: false)
    // Title: "When withdraw"
    .BDDfy();
```

## Available Chain Methods

| Method | Purpose |
|--------|---------|
| `.Given(...)` | Setup/Arrange step |
| `.When(...)` | Action/Act step |
| `.Then(...)` | Assertion step |
| `.And(...)` | Continuation of the previous step type |
| `.But(...)` | Contrasting continuation |
| `.TearDownWith(...)` | Cleanup step (runs even if earlier steps fail) |

Each method supports the following overloads:

- `Expression<Action<TScenario>>` — lambda pointing to a method on the test object
- `Expression<Func<TScenario, Task>>` — async lambda
- `Action` + `string title` — delegate with explicit title
- `Func<Task>` + `string title` — async delegate with explicit title
- `Expression<Func<ExampleAction>>` — for example-driven steps
- `string title` — title-only (no action, typically used with examples)

## Async Steps

The Fluent API natively supports `Task`-returning methods:

```csharp
this.Given(s => s.SetupAsync())
    .When(s => s.PerformActionAsync())
    .Then(s => s.VerifyAsync())
    .BDDfy();

// where:
async Task SetupAsync() { await Task.Delay(10); }
async Task PerformActionAsync() { /* ... */ }
async Task VerifyAsync() { /* ... */ }
```

## TearDown Steps

Use `.TearDownWith(...)` for cleanup that should always run:

```csharp
this.Given(s => s.OpenConnection())
    .When(s => s.ExecuteQuery())
    .Then(s => s.VerifyResults())
    .TearDownWith(s => s.CloseConnection())
    .BDDfy();
```

## Combining with Examples

The Fluent API integrates with `ExampleTable` for data-driven scenarios:

```csharp
this.Given("Given there are <start> cucumbers")
    .When(s => s.WhenIEat__eat__Cucumbers())
    .Then(s => s.ThenIShouldHave__left__Cucumbers())
    .WithExamples(new ExampleTable("Start", "Eat", "Left")
    {
        { 12, 5, 9 },
        { 20, 5, 17 }
    })
    .BDDfy();
```

→ See [Examples documentation](examples.md) for more details.

## Combining with Stories

Specify the story type as a generic argument to `BDDfy<TStory>()`:

```csharp
this.Given(s => s.Setup())
    .When(s => s.Action())
    .Then(s => s.Verify())
    .BDDfy<AtmWithdrawalStory>();
```

Or pass a report name:

```csharp
    .BDDfy(htmlReportName: "ATM");
```

## LazyBDDfy

Use `LazyBDDfy()` to get an `Engine` instance without immediately running the scenario. Useful for testing BDDfy itself or deferred execution:

```csharp
var engine = this.Given(s => s.Step())
                 .LazyBDDfy();

// Run later
engine.Run();
```
