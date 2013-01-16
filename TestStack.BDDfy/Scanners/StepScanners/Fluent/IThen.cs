using System;
using System.Linq.Expressions;

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
#endif
    }
}