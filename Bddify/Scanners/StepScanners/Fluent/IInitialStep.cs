using System;
using System.Linq.Expressions;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IInitialStep<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);

        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
#if NET35
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);
#endif
    }
}