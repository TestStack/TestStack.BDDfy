using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class ExecutableAttributeScanner : IScanForSteps
    {
        public IEnumerable<ExecutionStep> Scan(Type scenarioType)
        {
            var steps = new List<Tuple<ExecutableAttribute, ExecutionStep>>();
            foreach (var methodInfo in scenarioType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var executableAttribute = (ExecutableAttribute)methodInfo.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
                if(executableAttribute == null)
                    continue;

                string readableMethodName = executableAttribute.StepText;
                if(string.IsNullOrEmpty(readableMethodName))
                    readableMethodName = NetToString.FromName(methodInfo.Name);

                var stepAsserts = IsAssertingByAttribute(methodInfo);

                var argSets = (RunStepWithArgsAttribute[])methodInfo.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                if (argSets == null || argSets.Length == 0)
                {
                    var executionStep = new ExecutionStep(methodInfo, null, readableMethodName, stepAsserts);
                    steps.Add(new Tuple<ExecutableAttribute, ExecutionStep>(executableAttribute, executionStep));
                    continue;
                }

                foreach (var argSet in argSets)
                {
                    readableMethodName += " with args (" + string.Join(", ", argSet) + ")";
                    var executionStep = new ExecutionStep(methodInfo, argSet.InputArguments, readableMethodName, stepAsserts);
                    steps.Add(new Tuple<ExecutableAttribute, ExecutionStep>(executableAttribute, executionStep));
                }
            }

            if (steps.Count > 0)
                Handled = true;

            return steps.OrderBy(s => s.Item1.Order).Select(s => s.Item2).ToList();
        }

        public bool Handled { get; private set; }

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