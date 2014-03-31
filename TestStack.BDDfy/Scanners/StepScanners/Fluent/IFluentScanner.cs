using System;
using System.Linq.Expressions;

namespace TestStack.BDDfy
{
    public interface IFluentScanner<TScenario> : IHasScanner
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        IFluentScanner<TScenario> TearDownWith(Expression<Func<TScenario, System.Threading.Tasks.Task>> tearDownStep);
    }
}
