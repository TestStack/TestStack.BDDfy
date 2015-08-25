# Examples


## Actions as examples
One of the benefits that BDDfy's approach has it its flexibility.

``` csharp
ExampleAction actionToPerform = null;
int valueShouldBe = 0;
var story = this.Given(_ => SomeSetup())
    .When(() => actionToPerform)
    .Then(_ => ShouldBe(valueShouldBe))
    .WithExamples(new ExampleTable("Action to perform", "Value should be")
    {
        { new ExampleAction("Do something", () => { _value = 42; }), 42 },
        { new ExampleAction("Do something else", () => { _value = 7; }), 7 }
    })
    .BDDfy();
```

When run you will get this:

```
Scenario: Can use actions in examples
	Given some setup
	When  <Action to perform>
	Then should be <Value should be>

Examples:
| Action to perform | Value should be |
| Do something      | 42              |
| Do something else | 7               |
```

This can make your BDD test incredibly flexible and powerful.
