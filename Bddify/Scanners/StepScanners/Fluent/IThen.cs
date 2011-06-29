using System;
using System.Linq.Expressions;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IThen<TScenario> : IFluentScanner<TScenario>
    {
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
#if NET35
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep);
#endif
    }
}