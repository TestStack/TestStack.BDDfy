using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IThenSteps<TScenario>
    {
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Func<TScenario, Task>> thenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Action thenStep, string title);
        IThen<TScenario> Then(string title);
    }
}