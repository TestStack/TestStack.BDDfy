# Async Support

BDDfy supports asynchronous step methods using both `Task`-returning methods and `async void` methods.

## Task-Returning Steps (Recommended)

The preferred approach is to return `Task` from your step methods:

```csharp
public class AsyncScenario
{
    private HttpClient _client = new();
    private HttpResponseMessage _response = null!;

    async Task GivenTheServiceIsRunning()
    {
        await _client.GetAsync("http://localhost/health");
    }

    async Task WhenIRequestTheResource()
    {
        _response = await _client.GetAsync("http://localhost/api/items");
    }

    async Task ThenTheResponseIsSuccessful()
    {
        _response.EnsureSuccessStatusCode();
    }

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

### With the Fluent API

```csharp
[Fact]
public void Execute()
{
    this.Given(s => s.GivenTheServiceIsRunning())
        .When(s => s.WhenIRequestTheResource())
        .Then(s => s.ThenTheResponseIsSuccessful())
        .BDDfy();
}
```

The fluent API has dedicated overloads accepting `Expression<Func<TScenario, Task>>`.

## Async Void Steps

BDDfy can also handle `async void` methods. It installs a custom `SynchronizationContext` to detect when the method completes:

```csharp
public class AsyncVoidSteps
{
    private object _sut = null!;

    async void GivenSomeAsyncSetup()
    {
        _sut = await CreateSut();
    }

    void ThenBddfyHasWaitedForSetupToComplete()
    {
        Assert.NotNull(_sut);
    }

    private static async Task<object> CreateSut()
    {
        await Task.Delay(500);
        return new object();
    }

    [Fact]
    public void Execute()
    {
        this.BDDfy();
    }
}
```

### Exception Handling in Async Void

Exceptions thrown in `async void` methods are captured and reported normally:

```csharp
async void WhenSomethingFails()
{
    await Task.Yield();
    throw new InvalidOperationException("Something went wrong");
}
```

BDDfy will catch this exception and mark the step as failed.

### Disabling Async Void Support

If async void detection causes issues in your environment:

```csharp
Configurator.AsyncVoidSupportEnabled = false;
```

## Async Delegate Steps (Fluent API)

Use `Func<Task>` with an explicit title:

```csharp
this.Given(async () => await SetupDatabase(), "Given the database is seeded")
    .When(async () => await CallApi(), "When the API is called")
    .Then(async () => await VerifyResult(), "Then the result is correct")
    .BDDfy();
```

## Best Practices

1. **Prefer `Task`-returning methods** over `async void` — they're easier to debug and don't require the special synchronization context
2. **Avoid `Task.Result` or `.Wait()`** in step methods — use `async`/`await` throughout to prevent deadlocks
3. **Use `async void` only** when retrofitting existing code or when the testing framework requires it
