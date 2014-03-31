using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IInitialStep<TScenario> : IWhenSteps<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle);
        IGiven<TScenario> Given(Expression<Func<TScenario, Task>> givenStep, string stepTextTemplate = null);
        IGiven<TScenario> Given(Expression<Func<TScenario, Task>> givenStep, bool includeInputsInStepTitle);
        IGiven<TScenario> Given(Action givenStep, string title);
        IGiven<TScenario> Given(string title);
    }
}
