using System;
using System.Linq.Expressions;

#if !NET35
using System.Threading.Tasks;
#endif

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
{
    public interface IThen<TScenario> : IFluentScanner<TScenario>
    {
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle);
#if NET35
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep);
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate);
#else
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
        IAndThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, string stepTextTemplate = null);
        IAndThen<TScenario> And(Expression<Func<TScenario, Task>> andThenStep, bool includeInputsInStepTitle);
#endif
    }
}