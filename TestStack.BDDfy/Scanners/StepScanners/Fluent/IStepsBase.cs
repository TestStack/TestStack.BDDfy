using System;
using System.Linq.Expressions;

namespace TestStack.BDDfy
{
    public interface IStepsBase<TScenario> : IHasScanner
    {
        IStepsBase<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        IStepsBase<TScenario> TearDownWith(Expression<Func<TScenario, System.Threading.Tasks.Task>> tearDownStep);
    }
}
