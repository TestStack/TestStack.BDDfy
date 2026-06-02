# Reflective API (Convention-Based)

The Reflective API is BDDfy's convention-over-configuration approach. You write methods with specific naming patterns or decorate them with attributes, and BDDfy discovers and executes them automatically.

## Method Naming Conventions

BDDfy scans your test class for methods whose names start with recognized prefixes. The method name is humanized into a readable step title.

### Supported Prefixes

| Prefix | Execution Order | Reports As |
|--------|----------------|------------|
| `Given` | SetupState | "Given ..." |
| `AndGiven` / `And_Given_` | ConsecutiveSetupState | "And ..." |
| `ButGiven` / `But_Given_` | ConsecutiveSetupState | "But ..." |
| `When` | Transition | "When ..." |
| `AndWhen` / `And_When_` | ConsecutiveTransition | "And ..." |
| `ButWhen` / `But_When_` | ConsecutiveTransition | "But ..." |
| `Then` | Assertion | "Then ..." |
| `AndThen` / `And_Then_` / `And` | ConsecutiveAssertion | "And ..." |
| `ButThen` / `But_Then_` / `But` | ConsecutiveAssertion | "But ..." |

### Special (non-reporting) conventions

| Prefix/Suffix | Purpose |
|---------------|---------|
| `Setup` | Initialization (runs first, not reported) |
| Methods ending with `Context` | Initialization (runs first, not reported) |
| `TearDown` | Cleanup (runs last, not reported) |

### Naming Styles

All three styles are equivalent:

```csharp
// PascalCase
void GivenTheAccountHasSufficientFunds() { }

// Underscored
void Given_the_account_has_sufficient_funds() { }

// camelCase
void givenTheAccountHasSufficientFunds() { }
```

### Complete Example

```csharp
using Xunit;
using TestStack.BDDfy;

public class AccountHolderWithdrawsCash
{
    private int _balance = 100;
    private int _dispensed;

    void GivenTheAccountBalanceIs100() { }

    void AndGivenTheCardIsValid() { }

    void WhenTheAccountHolderRequests20()
    {
        _dispensed = 20;
        _balance -= 20;
    }

    void ThenTheAtmShouldDispense20()
    {
        Assert.Equal(20, _dispensed);
    }

    void AndTheAccountBalanceShouldBe80()
    {
        Assert.Equal(80, _balance);
    }

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

Report output:

```
Scenario: Account holder withdraws cash
    Given the account balance is 100
      And the card is valid
    When the account holder requests 20
    Then the atm should dispense 20
      And the account balance should be 80
```

## Executable Attributes

For more control over step text and ordering, use the executable attributes instead of (or in addition to) naming conventions.

### Available Attributes

| Attribute | Execution Order |
|-----------|----------------|
| `[Given]` | SetupState |
| `[AndGiven]` | ConsecutiveSetupState |
| `[ButGiven]` | ConsecutiveSetupState |
| `[When]` | Transition |
| `[AndWhen]` | ConsecutiveTransition |
| `[ButWhen]` | ConsecutiveTransition |
| `[Then]` | Assertion |
| `[AndThen]` | ConsecutiveAssertion |
| `[But]` | ConsecutiveAssertion |

### Overriding Step Text

Pass a string to the attribute to override the humanized method name:

```csharp
[Given("Given the account balance is $10")]
void GivenAccountHasEnoughBalance()
{
    _card = new Card(true, 10);
}
```

### Controlling Order

Use the `Order` property when you need explicit ordering:

```csharp
[Given(Order = 1)]
void SetupAccount() { }

[Given(Order = 2)]
void SetupCard() { }
```

### Suppressing a Step from Reports

```csharp
[Given(ShouldReport = false)]
void SomeInternalSetup() { }
```

### The Generic ExecutableAttribute

All GWT attributes inherit from `ExecutableAttribute`. You can use it directly:

```csharp
[Executable(ExecutionOrder.SetupState, "Custom step title")]
void MyCustomStep() { }
```

## RunStepWithArgs

Run the same step method multiple times with different arguments:

```csharp
[RunStepWithArgs(1, 2, StepTextTemplate = "Given {0} plus {1}")]
[RunStepWithArgs(3, 4, StepTextTemplate = "Given {0} plus {1}")]
void GivenAddition(int a, int b)
{
    _result = a + b;
}
```

## IgnoreStep Attribute

Exclude a method from step scanning even if its name matches a convention:

```csharp
[IgnoreStep]
void GivenThisIsAHelperNotAStep() { }
```

## Combining Conventions and Attributes

You can mix both approaches in the same class. Attributes take priority for the methods they decorate; remaining methods are matched by naming convention.

```csharp
public class MixedApproach
{
    [Given("Given a specific balance of $50")]
    void SetupBalance() { }

    // Discovered by naming convention
    void WhenTheUserWithdraws30() { }

    [Then]
    void ThenBalanceIs20() { }

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

## Custom Scenario Title

Override the default scenario title (derived from the class name) by passing it to `BDDfy()`:

```csharp
[Fact]
public void Execute()
{
    this.BDDfy("Account holder withdraws more than the balance");
}
```

## Methods Returning Step Text

If a step method returns a `string`, BDDfy uses the return value as the step title:

```csharp
string GivenTheBalance()
{
    _balance = 100;
    return "Given the balance is $100";
}
```
