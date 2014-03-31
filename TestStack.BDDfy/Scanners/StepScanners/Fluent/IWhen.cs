using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IWhen<TScenario> : IStepsBase<TScenario>, IThenSteps<TScenario>
    {
        IWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, string stepTextTemplate = null);
        IWhen<TScenario> And(Expression<Func<TScenario, Task>> andWhenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> And(Action andWhenStep, string title);
        IWhen<TScenario> And(string title);
    }
}