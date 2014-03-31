using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IThen<TScenario> : IFluentScanner<TScenario>
    {
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle);
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
        
        IAndThen<TScenario> And(Action andThenStep, string title);
        IAndThen<TScenario> And(string title);
        IAndThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, string stepTextTemplate = null);
        IAndThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, bool includeInputsInStepTitle);
    }
}
