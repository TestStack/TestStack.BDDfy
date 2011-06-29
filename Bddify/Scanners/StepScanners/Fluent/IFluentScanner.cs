using System;
using System.Linq.Expressions;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IFluentScanner<TScenario> : IScanForSteps
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        Story Bddify(string title = null, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true);
        Bddifier LazyBddify(string title = null);
#if NET35
        Bddifier LazyBddify();
        Story Bddify(string title);
        Story Bddify();
#endif
    }
}