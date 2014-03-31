using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IGiven<TScenario>
    {
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Action whenStep, string title);
        IWhen<TScenario> When(string title);

        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle);
        IAndGiven<TScenario> And(Action andGivenStep, string title);
        IAndGiven<TScenario> And(string title);
        
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);

        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);

        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, string stepTextTemplate = null);

        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, bool includeInputsInStepTitle);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, bool includeInputsInStepTitle);
    }
}