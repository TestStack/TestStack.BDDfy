using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute
{
    /// <summary>
    /// Uses reflection to scan scenario class for steps by looking for
    /// ExecutableAttribute on methods
    /// </summary>
    /// <remarks>
    /// You can use attributes either when your method name does not comply with the
    /// conventions or when you want to provide a step text that reflection would not be
    /// able to create for you. You can override step text using executable attributes.
    /// </remarks>
    /// <example>
    /// <code>
    /// [Given(StepTitle = "Given the account balance is $10")]
    /// void GivenTheAccountBalanceIs10()
    /// {
    ///    _card = new Card(true, 10);
    /// }
    /// </code>
    /// </example>
    public class ExecutableAttributeStepScanner : IStepScanner
    {
        public IEnumerable<ExecutionStep> Scan(object testObject, MethodInfo candidateMethod)
        {
            var executableAttribute = (ExecutableAttribute)candidateMethod.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
            if(executableAttribute == null)
                yield break;

            string stepTitle = executableAttribute.StepTitle;
            if(string.IsNullOrEmpty(stepTitle))
                stepTitle = NetToString.Convert(candidateMethod.Name);

            var stepAsserts = IsAssertingByAttribute(candidateMethod);

            var runStepWithArgsAttributes = (RunStepWithArgsAttribute[])candidateMethod.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
            if (runStepWithArgsAttributes.Length == 0)
            {
                yield return
                    new ExecutionStep(StepActionFactory.GetStepAction(candidateMethod, new object[0]), stepTitle, stepAsserts, executableAttribute.ExecutionOrder, true)
                        {
                            ExecutionSubOrder = executableAttribute.Order
                        };
            }

            foreach (var runStepWithArgsAttribute in runStepWithArgsAttributes)
            {
                var inputArguments = runStepWithArgsAttribute.InputArguments;
                var flatInput = inputArguments.FlattenArrays();
                var stringFlatInputs = flatInput.Select(i => i.ToString()).ToArray();
                var methodName = stepTitle + " " + string.Join(", ", stringFlatInputs);

                if (!string.IsNullOrEmpty(runStepWithArgsAttribute.StepTextTemplate))
                    methodName = string.Format(runStepWithArgsAttribute.StepTextTemplate, flatInput);
                else if (!string.IsNullOrEmpty(executableAttribute.StepTitle))
                    methodName = string.Format(executableAttribute.StepTitle, flatInput);

                yield return
                    new ExecutionStep(StepActionFactory.GetStepAction(candidateMethod, inputArguments), methodName, stepAsserts,
                                      executableAttribute.ExecutionOrder, true)
                        {
                            ExecutionSubOrder = executableAttribute.Order
                        };
            }
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