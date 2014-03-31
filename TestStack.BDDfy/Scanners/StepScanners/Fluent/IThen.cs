using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IThen<TScenario> : IStepsBase<TScenario>
    {
        IThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
        IThen<TScenario> And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle);
        IThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, string stepTextTemplate = null);
        IThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, bool includeInputsInStepTitle);
        IThen<TScenario> And(Action andThenStep, string title);
        IThen<TScenario> And(string title);
    }
}
