using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class ExecutableAttributeScanner : IScanForSteps
    {
        public int Priority
        {
            get { return 1; }
        }

        public IEnumerable<ExecutionStep> Scan(Type scenarioType)
        {
            var steps = new List<Tuple<ExecutableAttribute, ExecutionStep>>();
            foreach (var methodInfo in scenarioType.GetMethodsOfInterest())
            {
                var executableAttribute = (ExecutableAttribute)methodInfo.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
                if(executableAttribute == null)
                    continue;

                string readableMethodName = executableAttribute.StepText;
                if(string.IsNullOrEmpty(readableMethodName))
                    readableMethodName = NetToString.FromName(methodInfo.Name);

                var stepAsserts = IsAssertingByAttribute(methodInfo);

                var runStepWithArgsAttributes = (RunStepWithArgsAttribute[])methodInfo.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                if (runStepWithArgsAttributes == null || runStepWithArgsAttributes.Length == 0)
                {
                    var executionStep = new ExecutionStep(methodInfo, null, readableMethodName, stepAsserts, executableAttribute.ExecutionOrder);
                    steps.Add(new Tuple<ExecutableAttribute, ExecutionStep>(executableAttribute, executionStep));
                    continue;
                }

                foreach (var runStepWithArgsAttribute in runStepWithArgsAttributes)
                {
                    var methodName = readableMethodName + " " + string.Join(", ", runStepWithArgsAttribute.InputArguments);

                    if(!string.IsNullOrEmpty(runStepWithArgsAttribute.StepTextTemplate))
                        methodName = string.Format(runStepWithArgsAttribute.StepTextTemplate, runStepWithArgsAttribute.InputArguments);

                    var executionStep = new ExecutionStep(methodInfo, runStepWithArgsAttribute.InputArguments, methodName, stepAsserts, executableAttribute.ExecutionOrder);
                    steps.Add(new Tuple<ExecutableAttribute, ExecutionStep>(executableAttribute, executionStep));
                }
            }

            return steps.Select(s => s.Item2).ToList();
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