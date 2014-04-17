using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public interface IWhenSteps<TScenario>
    {
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Func<TScenario, Task>> whenStep, bool includeInputsInStepTitle);
        IWhen<TScenario> When(Action whenStep, string title);
        IWhen<TScenario> When(string title);
    }
}