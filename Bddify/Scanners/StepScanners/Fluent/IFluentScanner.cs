using System;
using System.Linq.Expressions;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IFluentScanner<TScenario> : IScanner
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        Story Bddify(string title);
        Bddifier LazyBddify(string title, bool consoleReport = true, bool htmlReport = true);
        Bddifier LazyBddify();
        Story Bddify();
    }
}