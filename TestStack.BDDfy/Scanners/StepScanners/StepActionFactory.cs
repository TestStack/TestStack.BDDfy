using System;
using System.Reflection;

namespace TestStack.BDDfy.Scanners.StepScanners
{
    public class StepActionFactory
    {
        public static Action<object> GetStepAction(MethodInfo method, object[] inputs)
        {
            return o => method.Invoke(o, inputs);
        }

        public static Action<object> GetStepAction<TScenario>(Action<TScenario> action)
            where TScenario : class
        {
            return o => action((TScenario) o);
        }
    }
}