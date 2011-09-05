using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.ExecutableAttribute
{
    public class ExecutableAttributeStepScanner : IScanForSteps
    {
        public IEnumerable<ExecutionStep> Scan(MethodInfo candidateMethod)
        {
            var executableAttribute = (ExecutableAttribute)candidateMethod.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
            if(executableAttribute == null)
                yield break;

            string readableMethodName = executableAttribute.StepText;
            if(string.IsNullOrEmpty(readableMethodName))
                readableMethodName = NetToString.Convert(candidateMethod.Name);

            var stepAsserts = IsAssertingByAttribute(candidateMethod);

            var runStepWithArgsAttributes = (RunStepWithArgsAttribute[])candidateMethod.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
            if (runStepWithArgsAttributes.Length == 0)
            {
                yield return new ExecutionStep(GetStepAction(candidateMethod), readableMethodName, stepAsserts, executableAttribute.ExecutionOrder, true);
            }

            foreach (var runStepWithArgsAttribute in runStepWithArgsAttributes)
            {
                var inputArguments = runStepWithArgsAttribute.InputArguments;
                var flatInput = inputArguments.FlattenArrays();
                var methodName = readableMethodName + " " + string.Join(", ", flatInput);

                if (!string.IsNullOrEmpty(runStepWithArgsAttribute.StepTextTemplate))
                    methodName = string.Format(runStepWithArgsAttribute.StepTextTemplate, flatInput);
                else if (!string.IsNullOrEmpty(executableAttribute.StepText))
                    methodName = string.Format(executableAttribute.StepText, flatInput);

                yield return new ExecutionStep(GetStepAction(candidateMethod, inputArguments), methodName, stepAsserts, executableAttribute.ExecutionOrder, true);
            }
        }

        static Action<object> GetStepAction(MethodInfo methodinfo, object[] inputArguments = null)
        {
            return o => methodinfo.Invoke(o, inputArguments);
        }

        private static bool IsAssertingByAttribute(MethodInfo method)
        {
            var attribute = GetExecutableAttribute(method);
            return attribute.Asserts;
        }

        private static ExecutableAttribute GetExecutableAttribute(MethodInfo method)
        {
            return (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).First();
        }
    }
}