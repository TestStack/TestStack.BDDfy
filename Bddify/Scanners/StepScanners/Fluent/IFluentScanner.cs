using System;
using System.Linq.Expressions;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.Fluent
{
    public interface IFluentScanner<TScenario> : IHasScanner
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
    }
}