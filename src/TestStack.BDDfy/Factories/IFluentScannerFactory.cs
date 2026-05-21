namespace TestStack.BDDfy.Factories;

public interface IFluentScannerFactory
{
    IFluentScanner Create<TScenario>(TScenario testObject) where TScenario : class;
}
