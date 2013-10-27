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
    }
}