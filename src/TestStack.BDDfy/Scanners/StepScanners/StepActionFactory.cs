using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public class StepActionFactory
    {
        public static Func<object, object?> GetStepAction(MethodInfo method, object[] inputs) => o => method.Invoke(o, inputs);

        public static Func<object, object?> GetStepAction<TScenario>(Action<TScenario> action)
            where TScenario : class => o => { action((TScenario)o); return null; };

        public static Func<object, object> GetStepAction<TScenario>(Func<TScenario, Task> action)
           where TScenario : class => o => action((TScenario)o);
    }
}