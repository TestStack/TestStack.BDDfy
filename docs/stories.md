# Stories

Stories let you group related scenarios under a shared narrative. BDDfy supports the standard user-story format: "As a … I want … So that …".

## The Story Attribute

Apply `[Story]` to a class to associate it with a user story:

```csharp
[Story(
    AsA = "As an account holder",
    IWant = "I want to withdraw cash from an ATM",
    SoThat = "So that I can get money when the bank is closed")]
public class AccountHasInsufficientFund
{
    // scenario steps...

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

Report output:

```
Story: Account has insufficient fund
    As an account holder
    I want to withdraw cash from an ATM
    So that I can get money when the bank is closed

  Scenario: Account has insufficient fund
      Given ...
      When ...
      Then ...
```

### Prefix Handling

BDDfy automatically prepends the expected prefix if you omit it:

```csharp
// Both are equivalent:
AsA = "As an account holder"
AsA = "an account holder"       // "As a" is prepended automatically
```

Same for `IWant` ("I want") and `SoThat` ("So that").

## Custom Title

Override the story title (defaults to the humanized class name):

```csharp
[Story(
    Title = "ATM Cash Withdrawal",
    AsA = "As an account holder",
    IWant = "I want to withdraw cash",
    SoThat = "So that I can access my money")]
public class AtmWithdrawal { }
```

## Title Prefix

Change the prefix displayed before the story title (defaults to `"Story: "`):

```csharp
[Story(TitlePrefix = "Feature: ", AsA = "...", IWant = "...", SoThat = "...")]
public class MyFeature { }
```

## StoryUri and ImageUri

Link to an external story tracker or display an image in HTML reports:

```csharp
[Story(
    AsA = "As a user",
    IWant = "I want to log in",
    SoThat = "So that I can access my account",
    StoryUri = "https://jira.example.com/browse/PROJ-123",
    ImageUri = "https://example.com/login-flow.png")]
public class UserLogin { }
```

## The StoryNarrativeAttribute (Generic Narratives)

`StoryAttribute` is a convenience wrapper over `StoryNarrativeAttribute`. For non-user-story narratives (e.g., "In order to … As a … I want …"), use `StoryNarrativeAttribute` directly:

```csharp
[StoryNarrative(
    TitlePrefix = "Feature: ",
    Title = "Login",
    Narrative1 = "In order to access my account",
    Narrative2 = "As a registered user",
    Narrative3 = "I want to authenticate with credentials")]
public class LoginFeature { }
```

## Shared Story Class

Multiple scenario classes can reference the same story by using `BDDfy<TStory>()`:

```csharp
// The story definition (no test code needed)
[Story(
    AsA = "As a player",
    IWant = "I want to play tic-tac-toe",
    SoThat = "So that I can have fun")]
public class TicTacToeStory { }

// Scenario 1
public class WhenXGoesFirst
{
    // steps...

    [Fact]
    public void Execute()
    {
        this.BDDfy<TicTacToeStory>();
    }
}

// Scenario 2
public class WhenOGoesFirst
{
    // steps...

    [Fact]
    public void Execute()
    {
        this.BDDfy<TicTacToeStory>();
    }
}
```

Both scenarios will appear under the same story in HTML reports.

## Shared Story with Fluent API

```csharp
this.Given(s => s.Setup())
    .When(s => s.Action())
    .Then(s => s.Verify())
    .BDDfy<TicTacToeStory>();
```

## Standalone Scenarios (No Story)

If no `[Story]` attribute is present and no `TStory` generic is used, BDDfy uses the test class namespace as a grouping key in reports:

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

## Custom Story Attribute

You can create your own story attribute by inheriting from `StoryNarrativeAttribute`:

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class FeatureAttribute : StoryNarrativeAttribute
{
    public string InOrderTo
    {
        get => Narrative1;
        set => Narrative1 = value;
    }

    public string AsA
    {
        get => Narrative2;
        set => Narrative2 = value;
    }

    public string IWant
    {
        get => Narrative3;
        set => Narrative3 = value;
    }
}
```
