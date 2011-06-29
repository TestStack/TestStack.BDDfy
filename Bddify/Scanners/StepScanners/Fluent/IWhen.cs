using System;
using System.Linq.Expressions;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IWhen<TScenario> : IFluentScanner<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
#if NET35
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep);
#endif
    }
}