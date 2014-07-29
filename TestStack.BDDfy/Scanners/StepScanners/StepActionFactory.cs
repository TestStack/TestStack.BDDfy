using System;
using System.Reflection;
using System.Security;
using System.Threading;
using TestStack.BDDfy.Processors;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public class StepActionFactory
    {
        public static Func<object,object> GetStepAction(MethodInfo method, object[] inputs)
        {
            return o => method.Invoke(o, inputs);
        }

        public static Func<object,object> GetStepAction<TScenario>(Action<TScenario> action)
            where TScenario : class
        {
            return o => 
            {
                action((TScenario)o);
                return null;
            };
        }

        public static Func<object,object> GetStepAction<TScenario>(Func<TScenario, Task> action)
           where TScenario : class
        {
            return o => action((TScenario)o);
        }
    }
}