using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IWhen<TScenario> : IStepsBase<TScenario>, IThenSteps<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle);
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IAndWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, bool includeInputsInStepTitle);
        IAndWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, string stepTextTemplate = null);
        IAndWhen<TScenario> And(Action andWhenStep, string title);
        IAndWhen<TScenario> And(string title);
    }
}