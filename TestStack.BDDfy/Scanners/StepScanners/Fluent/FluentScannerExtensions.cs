using System;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace
// This is in BDDfy namespace to make its usage simpler
namespace TestStack.BDDfy
// ReSharper restore CheckNamespace
{
    public static class FluentStepScannerExtensions
    {
        static IInitialStep<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class
        {
            return new FluentScanner<TScenario>(testObject);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, string stepTextTemplate)
            where TScenario: class
        {
            return testObject.Scan().Given(givenStep, stepTextTemplate);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return testObject.Scan().Given(givenStep, includeInputsInStepTitle);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, string stepTextTemplate)
            where TScenario : class
        {
            return testObject.Scan().When(whenStep, stepTextTemplate);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
            where TScenario : class
        {
            return testObject.Scan().When(whenStep, includeInputsInStepTitle);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep)
            where TScenario: class
        {
            return testObject.Given(givenStep, null);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep)
            where TScenario : class
        {
            return testObject.When(whenStep, null);
        }
    }
}