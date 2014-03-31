using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IGiven<TScenario> : IWhenSteps<TScenario>, IThenSteps<TScenario>
    {
        IGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle);
        IGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, string stepTextTemplate = null);
        IGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, bool includeInputsInStepTitle);
        IGiven<TScenario> And(Action andGivenStep, string title);
        IGiven<TScenario> And(string title);
    }
}