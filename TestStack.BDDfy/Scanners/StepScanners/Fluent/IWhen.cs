using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IWhen<TScenario> : IFluentScanner<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);

        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);

        IAndWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, string stepTextTemplate = null);

        IAndWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, bool includeInputsInStepTitle);
    }
}