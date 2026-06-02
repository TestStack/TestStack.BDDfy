![Build Status](https://github.com/TestStack/TestStack.BDDfy/actions/workflows/build.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/TestStack.BDDfy.svg)](https://www.nuget.org/packages/TestStack.BDDfy)

# BDDfy

**The simplest BDD framework for .NET — easy to use, customize, and extend.**

## Key Features

- **Works with any test framework** — xUnit, NUnit, MSTest, or plain POCO classes
- **No special test runner** — use your IDE, `dotnet test`, or any runner you prefer
- **Two flexible APIs** — convention-based (reflective) or explicit (fluent)
- **Rich reporting** — Console, HTML (Classic & Metro), Markdown, Text, and JSON diagnostics
- **Data-driven scenarios** — `ExampleTable` for parameterized tests
- **Stories are optional** — group scenarios under stories or run them standalone
- **Fully extensible** — custom reporters, scanners, step executors, and humanizers
- **Async support** — `Task`-returning and `async void` step methods

## Quick Start

```shell
dotnet add package TestStack.BDDfy
```

### Reflective API (convention-based)

Name your methods with Given/When/Then prefixes:

```csharp
public class ShouldRefundItem
{
	void GivenTheItemWasBoughtRecently() { }
	void WhenTheCustomerReturnsIt() { }
	void ThenARefundIsIssued() { }

	[Fact]
	public void Execute() => this.BDDfy();
}
```

### Fluent API (explicit)

```csharp
[Fact]
public void CardHasBeenDisabled()
{
	this.Given(s => s.GivenTheCardIsDisabled())
		.When(s => s.WhenTheAccountHolderRequests(20))
		.Then(s => s.ThenTheAtmRetainsTheCard())
		.BDDfy();
}
```

Both produce readable reports:

```
Scenario: Should refund item
	Given the item was bought recently
	When the customer returns it
	Then a refund is issued
```

## 📖 Documentation

Full documentation is available in the [`docs/`](docs/) folder:

| Guide | Description |
|-------|-------------|
| [Getting Started](docs/getting-started.md) | Installation and first scenario |
| [Reflective API](docs/reflective-api.md) | Method naming conventions and executable attributes |
| [Fluent API](docs/fluent-api.md) | Chainable Given/When/Then builder |
| [Stories](docs/stories.md) | Story metadata, shared stories, standalone scenarios |
| [Examples](docs/examples.md) | Data-driven scenarios with ExampleTable |
| [Reporters](docs/reporters.md) | Console, HTML, Markdown, Text, and Diagnostics reporters |
| [Configuration](docs/configuration.md) | Customizing the BDDfy pipeline |
| [Extensibility](docs/extensibility.md) | Custom reporters, scanners, and step executors |
| [Async Support](docs/async-support.md) | Async Task and async void steps |
| [Tags](docs/tags.md) | Tagging and filtering scenarios |

## IDE Annotations

Step-discovery attributes (`[Given]`, `[When]`, `[Then]`, etc.) are marked with `MeansImplicitUse` so IDEs like ReSharper and Rider won't flag step methods as unused. See [`src/TestStack.BDDfy/Properties/Annotations.cs`](src/TestStack.BDDfy/Properties/Annotations.cs) for details.

## Authors

* [Gurpreet Singh](https://github.com/SonOfSardaar)
* [Mehdi Khalili](https://github.com/MehdiK)
* [Michael Whelan](https://github.com/mwhelan)
* [Jake Ginnivan](https://github.com/JakeGinnivan)

## License

BDDfy is released under the MIT License. See the bundled [license.txt](license.txt) file for details.