using System;
using System.Linq.Expressions;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IInitialStep<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
#if NET35
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);
#endif
    }
}