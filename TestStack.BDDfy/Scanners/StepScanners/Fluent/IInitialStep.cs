using System;
using System.Linq.Expressions;

#if !NET35
using System.Threading.Tasks;
#endif

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
{
    public interface IInitialStep<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
#if NET35
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);

        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate);
#else
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);

        IGiven<TScenario> Given(Expression<Func<TScenario, Task>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, string stepTextTemplate = null);
        IGiven<TScenario> Given(Expression<Func<TScenario, Task>> givenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, bool includeInputsInStepTitle);
#endif
    }
}