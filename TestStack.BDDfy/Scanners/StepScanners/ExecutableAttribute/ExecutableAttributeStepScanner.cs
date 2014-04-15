using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TestStack.BDDfy
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
    /// [Given(Title = "Given the account balance is $10")]
    /// void GivenTheAccountBalanceIs10()
    /// {
    ///    _card = new Card(true, 10);
    /// }
    /// </code>
    /// </example>
    public class ExecutableAttributeStepScanner : IStepScanner
    {
        public IEnumerable<Step> Scan(object testObject, MethodInfo candidateMethod)
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
                    new Step(StepActionFactory.GetStepAction(candidateMethod, new object[0]), stepTitle, stepAsserts, executableAttribute.ExecutionOrder, true)
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
                    new Step(StepActionFactory.GetStepAction(candidateMethod, inputArguments), methodName, stepAsserts,
                                      executableAttribute.ExecutionOrder, true)
                        {
                            ExecutionSubOrder = executableAttribute.Order
                        };
            }
        }

        public IEnumerable<Step> Scan(object testObject, MethodInfo method, Example example)
        {
            var executableAttribute = (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
            if (executableAttribute == null)
                yield break;

            string stepTitle = executableAttribute.StepTitle;
            if (string.IsNullOrEmpty(stepTitle))
                stepTitle = NetToString.Convert(method.Name);

            var stepAsserts = IsAssertingByAttribute(method);
            var methodParameters = method.GetParameters();

            var inputs = new List<object>();
            var inputPlaceholders = Regex.Matches(stepTitle, " <\\w+> ");

            for (int i = 0; i < inputPlaceholders.Count; i++)
            {
                var placeholder = inputPlaceholders[i].Value;

                for (int j = 0; j < example.Headers.Length; j++)
                {
                    if (string.Format(" <{0}> ", example.Headers[j]).Equals(placeholder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        inputs.Add(example.GetValueOf(j, methodParameters[inputs.Count].ParameterType));
                        break;
                    }
                }

            }

            var stepAction = StepActionFactory.GetStepAction(method, inputs.ToArray());
            yield return new Step(stepAction, stepTitle, stepAsserts, executableAttribute.ExecutionOrder, true);
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