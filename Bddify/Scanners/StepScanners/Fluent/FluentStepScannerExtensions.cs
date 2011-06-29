namespace Bddify.Scanners.StepScanners.Fluent
{
#if !SILVERLIGHT
    public static class FluentStepScannerExtensions
    {
        public static IInitialStep<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class, new()
        {
            return new FluentStepScanner<TScenario>(testObject);
        }
    }
#endif
}