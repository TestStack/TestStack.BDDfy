using System;
using System.Linq.Expressions;

#if !NET35
using System.Threading.Tasks;
#endif

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
{
    public interface IGiven<TScenario>
    {
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);
#if NET35
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep);

        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate);
#else
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);

        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, string stepTextTemplate = null);

        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, bool includeInputsInStepTitle);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, bool includeInputsInStepTitle);
#endif
    }
}