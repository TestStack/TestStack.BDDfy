using System;
using System.Linq.Expressions;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IFluentScanner<TScenario> : IScanForSteps
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        Story Bddify(string title);
        Bddifier LazyBddify(string title);
        Bddifier LazyBddify();
        Story Bddify();
    }
}