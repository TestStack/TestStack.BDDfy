using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IGiven<TScenario> : IWhenSteps<TScenario>, IThenSteps<TScenario>
    {
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle);
        IAndGiven<TScenario> And(Action andGivenStep, string title);
        IAndGiven<TScenario> And(string title);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Func<TScenario, Task>> andGivenStep, bool includeInputsInStepTitle);
    }
}