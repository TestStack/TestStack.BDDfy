namespace TestStack.BDDfy.Factories;

internal class DefaultFluentScannerFactory : IFluentScannerFactory
{
    public IFluentScanner Create<TScenario>(TScenario testObject) where TScenario : class => new FluentScanner<TScenario>(testObject);
}
