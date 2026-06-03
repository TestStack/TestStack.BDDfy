# Examples (Data-Driven Scenarios)

BDDfy supports data-driven testing through `ExampleTable`. A single scenario definition runs once per row in the table, with placeholder values substituted into step titles and method arguments.

## ExampleTable Basics

An `ExampleTable` is defined with column headers and rows of values:

```csharp
var examples = new ExampleTable("Start", "Eat", "Left")
{
    { 12, 5, 7 },
    { 20, 5, 15 }
};
```

## With the Reflective API

Use `WithExamples()` before calling `BDDfy()`. Placeholders in step titles use `__header__` syntax in method names or `<header>` syntax in attribute text:

```csharp
public class UseExamplesWithReflectiveApi
{
    private int _start;
    private int _eat;

    [Fact]
    public void CanRunExamplesWithReflectiveApi()
    {
        this.WithExamples(new ExampleTable("Start", "Eat", "Left")
            {
                { 12, 5, 7 },
                { 20, 5, 15 }
            })
            .BDDfy();
    }

    // Double underscores in method name create a placeholder: <start>
    void GivenThereAre__start__Cucumbers(int start)
    {
        _start = start;
    }

    // Or use an attribute with <header> placeholder
    [AndGiven("And I eat <eat> of them")]
    void WhenIEatAFewCucumbers(int eat)
    {
        _eat = eat;
    }

    void ThenIShouldHave__left__Cucumbers(int left)
    {
        (_start - _eat).ShouldBe(left);
    }
}
```

### How Matching Works

1. BDDfy finds placeholders in the step title (e.g., `<start>`)
2. It looks for a matching column header in the `ExampleTable` (case-insensitive)
3. It assigns the value to the method parameter with the same name, or sets a matching property on the test class

## With the Fluent API

```csharp
public class UseExamplesWithFluentApi
{
    public int Start { get; set; }
    public int Eat { get; set; }
    public int Left { get; set; }

    [Fact]
    public void RunExamplesWithFluentApi()
    {
        this.Given("Given there are <start> cucumbers")
                .And(s => s.AndIStealTwoMore())
            .When(s => s.WhenIEat__eat__Cucumbers())
            .Then(s => s.ThenIShouldHave__left__Cucumbers())
            .WithExamples(new ExampleTable("Start", "Eat", "Left")
            {
                { 12, 5, 9 },
                { 20, 5, 17 }
            })
            .BDDfy();
    }

    private void AndIStealTwoMore() { Start += 2; }
    private void WhenIEat__eat__Cucumbers() { }
    private void ThenIShouldHave__left__Cucumbers()
    {
        (Start - Eat).ShouldBe(Left);
    }
}
```

Note: With the Fluent API you can use title-only steps (`Given("Given there are <start> cucumbers")`) — the framework sets the matching property automatically.

## Parsing a Table from a String

`ExampleTable` supports parsing pipe-delimited table strings:

```csharp
var table = ExampleTable.Parse(@"
    | Start | Eat | Left |
    | 12    | 5   | 7    |
    | 20    | 5   | 15   |
");

this.WithExamples(table).BDDfy();
```

## ExampleAction (Action Columns)

Use `ExampleAction` when a column should represent an action to execute rather than a data value:

```csharp
public class ExampleActionDemo
{
    public ExampleAction Action { get; set; }

    [Fact]
    public void Execute()
    {
        this.Given(s => s.Setup())
            .When(() => Action)
            .Then(s => s.Verify())
            .WithExamples(new ExampleTable("Action")
            {
                { new ExampleAction("Do first thing", () => DoFirst()) },
                { new ExampleAction("Do second thing", () => DoSecond()) }
            })
            .BDDfy();
    }
}
```

## Properties vs Method Parameters

BDDfy can inject example values into:

1. **Method parameters** — parameter name must match the column header
2. **Public properties** — property name must match the column header

If both exist, the method parameter takes precedence.

## Unused Example Values

If an example column is defined but never referenced in any step title or method parameter, BDDfy throws an `UnusedExampleException` to alert you to potential typos.
