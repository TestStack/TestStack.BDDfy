using Bddify.Scanners;

namespace Bddify
{
    public static class ScannerExtensions
    {
        public static FluentStepScanner<TScenario> Scan<TScenario>(this TScenario testObject)
        {
            return new FluentStepScanner<TScenario>(testObject);
        }
    }
}