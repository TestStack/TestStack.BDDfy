using System;
using System.Linq.Expressions;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
{
    public interface IFluentScanner<TScenario> : IHasScanner
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
#if !NET35
        IFluentScanner<TScenario> TearDownWith(Expression<Func<TScenario, System.Threading.Tasks.Task>> tearDownStep);
#endif
    }
}