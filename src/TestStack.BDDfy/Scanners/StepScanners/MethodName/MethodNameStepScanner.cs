using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Uses reflection to scan a scenario class for steps using method name conventions.
    /// </summary>
    /// <remarks>
    /// Method names starting with the following words are considered as steps and are reported:
    /// Given, AndGiven, When, AndWhen, Then, And.
    /// A method ending with "Context" or starting with "Setup" is a setup method (not reported).
    /// A method starting with "TearDown" is a finally method run after all other steps (not reported).
    /// </remarks>
    public class MethodNameStepScanner : IStepScanner
    {
        private readonly Func<string?, string> _stepTextTransformer;
        private readonly List<MethodNameMatcher> _matchers;

        public MethodNameStepScanner(Func<string?, string> stepTextTransformer, params MethodNameMatcher[] matchers)
        {
            _stepTextTransformer = stepTextTransformer;
            _matchers = [.. matchers];
        }

        public MethodNameStepScanner(Func<string?, string> stepTextTransformer)
        {
            _stepTextTransformer = stepTextTransformer;
            _matchers = [];
        }

        protected void AddMatcher(MethodNameMatcher matcher) => _matchers.Add(matcher);

        public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo method)
        {
            foreach (var matcher in _matchers)
            {
                if (!matcher.IsMethodOfInterest(method.Name))
                    continue;

                foreach (var step in ScanWithArgs(testContext, matcher, method))
                    yield return step;

                yield break;
            }
        }

        public IEnumerable<Step> Scan(ITestContext testContext, MethodInfo method, Example example)
        {
            foreach (var matcher in _matchers.Where(m => m.IsMethodOfInterest(method.Name)))
                return ScanWithArgs(testContext, matcher, method, example);

            return [];
        }

        private IEnumerable<Step> ScanWithArgs(ITestContext testContext, MethodNameMatcher matcher, MethodInfo method, Example? example = null)
        {
            var returnsItsText = ReturnsStepTitle(method);
            var argAttributes = method.GetCustomAttributes<RunStepWithArgsAttribute>(false).ToArray();

            if (argAttributes.Length == 0)
            {
                yield return BuildStep(testContext, matcher, method, returnsItsText, example, argAttribute: null);
                yield break;
            }

            foreach (var attr in argAttributes.Where(a => a.InputArguments is { Length: > 0 }))
                yield return BuildStep(testContext, matcher, method, returnsItsText, example, attr);
        }

        private Step BuildStep(
            ITestContext testContext,
            MethodNameMatcher matcher,
            MethodInfo method,
            bool returnsItsText,
            Example? example,
            RunStepWithArgsAttribute? argAttribute)
        {
            var inputs = ResolveInputs(method, example, argAttribute);
            var stepTitle = ResolveTitle(testContext, matcher, method, argAttribute, returnsItsText, inputs);
            var stepAction = CreateStepAction(method, inputs, returnsItsText);

            return new Step(stepAction, stepTitle, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport, [])
            {
                AllowConsecutivePromotion = true
            };
        }

        private static object[] ResolveInputs(MethodInfo method, Example? example, RunStepWithArgsAttribute? argAttribute)
        {
            if (example is null)
                return argAttribute?.InputArguments ?? [];

            var parameters = method.GetParameters();
            var inputs = new object[parameters.Length];
            var runStepArgs = argAttribute?.InputArguments ?? [];
            var runStepArgIndex = 0;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (TryResolveFromExample(parameters[i], example, out var value))
                {
                    inputs[i] = value;
                }
                else if (runStepArgIndex < runStepArgs.Length)
                {
                    inputs[i] = runStepArgs[runStepArgIndex++];
                }
            }

            return inputs;
        }

        private static bool TryResolveFromExample(ParameterInfo parameter, Example example, out object value)
        {
            value = null!;
            for (var i = 0; i < example.Headers.Length; i++)
            {
                if (example.Values.ElementAt(i).MatchesName(parameter.Name)
                    && example.GetValueOf(i, parameter.ParameterType) is { } resolved)
                {
                    value = resolved;
                    return true;
                }
            }
            return false;
        }

        private StepTitle ResolveTitle(
            ITestContext testContext,
            MethodNameMatcher matcher,
            MethodInfo method,
            RunStepWithArgsAttribute? argAttribute,
            bool returnsItsText,
            object[] inputs)
        {
            if (returnsItsText)
            {
                var titleFromMethod = InvokeForTitle(method, argAttribute, testContext.TestObject);
                if (titleFromMethod is not null)
                    return new StepTitle(titleFromMethod);
            }

            return CreateStepTitle(testContext, matcher, method, argAttribute, inputs);
        }

        private StepTitle CreateStepTitle(
            ITestContext testContext,
            MethodNameMatcher matcher,
            MethodInfo method,
            RunStepWithArgsAttribute? argAttribute,
            object[] inputs)
        {
            var stepTextTemplate = argAttribute?.StepTextTemplate;
            var stepArgs = inputs.Select(v => new StepArgument(() => v)).ToArray();

            var titleAttribute = method.GetCustomAttribute<StepTitleAttribute>(true);
            if (titleAttribute is not null)
            {
                var hasExplicitText = !string.IsNullOrWhiteSpace(titleAttribute.StepTitle) || !string.IsNullOrEmpty(stepTextTemplate);
                var prefix = hasExplicitText ? "" : matcher.StepPrefix;
                return Configurator.StepTitleFactory.Create(stepTextTemplate, null, method, stepArgs, testContext, prefix);
            }

            if (!string.IsNullOrEmpty(stepTextTemplate))
                return Configurator.StepTitleFactory.Create(stepTextTemplate, null, method, stepArgs, testContext, "");

            var humanized = _stepTextTransformer(Configurator.Humanizer.Humanize(method.Name));
            return Configurator.StepTitleFactory.Create(humanized, null, method, stepArgs, testContext, matcher.StepPrefix);
        }

        private static string? InvokeForTitle(MethodInfo method, RunStepWithArgsAttribute? argAttribute, object testObject)
        {
            try
            {
                var result = method.Invoke(testObject, argAttribute?.InputArguments ?? []);
                return result switch
                {
                    string s => s,
                    IEnumerable<string> enumerable => enumerable.FirstOrDefault(),
                    _ => null
                };
            }
            catch (Exception ex)
            {
                throw new StepTitleException(
                    $"The signature of method '{method.Name}' indicates that it returns its step title; " +
                    "but the code is throwing an exception before a title is returned", ex);
            }
        }

        private static bool ReturnsStepTitle(MethodInfo method) =>
            method.ReturnType == typeof(string) || method.ReturnType == typeof(IEnumerable<string>);

        private static Func<object, object?> CreateStepAction(MethodInfo method, object[] inputs, bool returnsItsText)
        {
            if (!returnsItsText)
                return StepActionFactory.GetStepAction(method, inputs);

            return o =>
            {
                var result = method.Invoke(o, inputs);
                if (result is IEnumerable<string> enumerable)
                    return enumerable.ToList();
                return result;
            };
        }
    }
}
