using System;
using System.Linq.Expressions;
using Bddify.Scanners.StepScanners.Fluent;

// ReSharper disable CheckNamespace
// This is in Bddify namespace to make its usage simpler
namespace Bddify
// ReSharper restore CheckNamespace
{
#if !SILVERLIGHT
    public static class FluentStepScannerExtensions
    {
        static IInitialStep<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class, new()
        {
            return new FluentStepScanner<TScenario>(testObject);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, string stepTextTemplate)
            where TScenario: class, new()
        {
            return testObject.Scan().Given(givenStep, stepTextTemplate);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, string stepTextTemplate)
            where TScenario : class, new()
        {
            return testObject.Scan().When(whenStep, stepTextTemplate);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep)
            where TScenario: class, new()
        {
            return testObject.Given(givenStep, null);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep)
            where TScenario : class, new()
        {
            return testObject.When(whenStep, null);
        }
    }
#endif
}