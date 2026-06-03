using System.Reflection;
using System.Text.RegularExpressions;
using TestStack.BDDfy.Configuration;

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
        public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo candidateMethod)
        {
            var executableAttribute = candidateMethod.GetCustomAttribute<ExecutableAttribute>(true);
            if (executableAttribute == null)
                yield break;

            var shouldReport = executableAttribute.ShouldReport;
            var stepPrefix = GetStepPrefix(executableAttribute);

            var runStepWithArgsAttributes = (RunStepWithArgsAttribute[])candidateMethod.GetCustomAttributes(typeof(RunStepWithArgsAttribute), true);
            if (runStepWithArgsAttributes.Length == 0)
            {
                var stepArgs = Array.Empty<StepArgument>();
                var template = string.IsNullOrWhiteSpace(executableAttribute.StepTitle) ? null : executableAttribute.StepTitle;
                var isPrimaryStep = IsPrimaryExecutionOrder(executableAttribute.ExecutionOrder);
                var effectivePrefix = template != null && !isPrimaryStep ? "" : stepPrefix;
                var stepTitle = Configurator.StepTitleFactory.Create(template, null, candidateMethod, stepArgs, testContext, effectivePrefix);
                var stepAction = StepActionFactory.GetStepAction(candidateMethod, []);
                yield return new Step(
                    stepAction,
                    stepTitle,
                    executableAttribute.Asserts,
                    executableAttribute.ExecutionOrder,
                    shouldReport,
                    [])
                {
                    ExecutionSubOrder = executableAttribute.Order,
                    AllowConsecutivePromotion = true
                };
            }

            foreach (var runStepWithArgsAttribute in runStepWithArgsAttributes)
            {
                var inputArguments = runStepWithArgsAttribute.InputArguments;
                var hasRunStepTemplate = !string.IsNullOrEmpty(runStepWithArgsAttribute.StepTextTemplate);
                var stepTextTemplate = hasRunStepTemplate
                    ? runStepWithArgsAttribute.StepTextTemplate
                    : (string.IsNullOrWhiteSpace(executableAttribute.StepTitle) ? null : executableAttribute.StepTitle);
                var stepArgs = inputArguments.Select(v => new StepArgument(() => v)).ToArray();
                var isPrimaryStep = IsPrimaryExecutionOrder(executableAttribute.ExecutionOrder);
                var effectivePrefix = stepTextTemplate != null && (!isPrimaryStep || hasRunStepTemplate) ? "" : stepPrefix;
                var stepTitle = Configurator.StepTitleFactory.Create(stepTextTemplate, null, candidateMethod, stepArgs, testContext, effectivePrefix);

                var stepAction = StepActionFactory.GetStepAction(candidateMethod, inputArguments);
                yield return new Step(
                    stepAction,
                    stepTitle,
                    executableAttribute.Asserts,
                    executableAttribute.ExecutionOrder,
                    shouldReport,
                    [])
                {
                    ExecutionSubOrder = executableAttribute.Order,
                    AllowConsecutivePromotion = true
                };
            }
        }

        public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo method, Example example)
        {
            var executableAttribute = method.GetCustomAttribute<ExecutableAttribute>(true);
            if (executableAttribute == null)
                yield break;

            var stepPrefix = GetStepPrefix(executableAttribute);
            var hasExplicitTitle = !string.IsNullOrWhiteSpace(executableAttribute.StepTitle);
            var stepTitle = executableAttribute.StepTitle;
            if (!hasExplicitTitle && Configurator.Humanizer.Humanize(method.Name) is string humanizedName)
                stepTitle = humanizedName;

            var shouldReport = executableAttribute.ShouldReport;
            var methodParameters = method.GetParameters();

            var inputs = new List<object>();
            if(stepTitle is not null)
            {
                var inputPlaceholders = Regex.Matches(stepTitle, " <(\\w+)> ");

                for (int i = 0; i < inputPlaceholders.Count; i++)
                {
                    var placeholder = inputPlaceholders[i].Groups[1].Value;

                    for (int j = 0; j < example.Headers.Length; j++)
                    {
                        if (example.Values.ElementAt(j).MatchesName(placeholder) && example.GetValueOf(j, methodParameters[inputs.Count].ParameterType) is object value)
                        {
                            inputs.Add(value);
                            break;
                        }
                    }
                }
            }

            var stepAction = StepActionFactory.GetStepAction(method, [.. inputs]);
            var isPrimaryStep = IsPrimaryExecutionOrder(executableAttribute.ExecutionOrder);
            var effectivePrefix = hasExplicitTitle && !isPrimaryStep ? "" : stepPrefix;
            var finalTitle = Configurator.StepTitleFactory.Create(stepTitle ?? string.Empty, effectivePrefix, testContext);
            yield return new Step(
                stepAction,
                finalTitle,
                executableAttribute.Asserts,
                executableAttribute.ExecutionOrder,
                shouldReport,
                [])
            {
                AllowConsecutivePromotion = true
            };
        }

        private static string GetStepPrefix(ExecutableAttribute attribute) => attribute switch
        {
            GivenAttribute => "Given",
            AndGivenAttribute => "And",
            ButGivenAttribute => "But",
            WhenAttribute => "When",
            AndWhenAttribute => "And",
            ButWhenAttribute => "But",
            ThenAttribute => "Then",
            AndThenAttribute => "And",
            ButAttribute => "But",
            _ => ""
        };

        private static bool IsPrimaryExecutionOrder(ExecutionOrder order) =>
            order is ExecutionOrder.SetupState or ExecutionOrder.Transition or ExecutionOrder.Assertion;
    }
}