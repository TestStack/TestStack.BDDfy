using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners.StepScanners.ExecutableAttribute
{
    public class ExecutableAttributeStepScanner : IScanForSteps
    {
        public int Priority
        {
            get { return 10; }
        }

        public IEnumerable<ExecutionStep> Scan(object testObject)
        {
            var scenarioType = testObject.GetType();
            var steps = new List<ExecutionStep>();
            foreach (var methodInfo in scenarioType.GetMethodsOfInterest())
            {
                var executableAttribute = (ExecutableAttribute)methodInfo.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
                if(executableAttribute == null)
                    continue;

                string readableMethodName = executableAttribute.StepText;
                if(string.IsNullOrEmpty(readableMethodName))
                    readableMethodName = NetToString.Convert(methodInfo.Name);

                var stepAsserts = IsAssertingByAttribute(methodInfo);

                var runStepWithArgsAttributes = (RunStepWithArgsAttribute[])methodInfo.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                if (runStepWithArgsAttributes.Length == 0)
                {
                    var executionStep = new ExecutionStep(methodInfo, null, readableMethodName, stepAsserts, executableAttribute.ExecutionOrder, true);
                    steps.Add(executionStep);
                    continue;
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

                    var executionStep = new ExecutionStep(methodInfo, inputArguments, methodName, stepAsserts, executableAttribute.ExecutionOrder, true);
                    steps.Add(executionStep);
                }
            }

            return steps;
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