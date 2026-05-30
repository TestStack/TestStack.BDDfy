using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Scanners.ScenarioScanners;

namespace TestStack.BDDfy
{
    internal class FluentScanner<TScenario> : IFluentScanner
        where TScenario : class
    {
        private readonly List<Step> _steps = [];
        private readonly TScenario _testObject;
        private readonly ITestContext _testContext;
        private readonly MethodInfo _fakeExecuteActionMethod;

        internal FluentScanner(TScenario testObject)
        {
            _testObject = testObject;
            _testContext = TestContext.GetContext(_testObject);
            _fakeExecuteActionMethod = typeof(FluentScanner<TScenario>)
                .GetMethod(nameof(ExecuteAction), BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Failed to retrieve method info for ExecuteAction.");
        }

        public IScanner GetScanner(string? scenarioTitle, Type? explicitStoryType)
        {
            return new DefaultScanner(_testContext, new FluentScenarioScanner(_steps, scenarioTitle), explicitStoryType);
        }

        public void AddStep(Action stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var action = StepActionFactory.GetStepAction<object>(o => stepAction());
            var stepTitle = CreateTitle(title, stepPrefix);
            _steps.Add(new Step(action, stepTitle, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports, []));
        }

        public void AddStep(Func<Task> stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var action = StepActionFactory.GetStepAction<object>(o => stepAction());
            var stepTitle = CreateTitle(title, stepPrefix);
            _steps.Add(new Step(action, stepTitle, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports, []));
        }

        private StepTitle CreateTitle(string title, string stepPrefix) => Configurator.StepTitleFactory.Create(title, stepPrefix, _testContext);

        public void AddStep(Expression<Func<ExampleAction>> stepAction, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var compiledAction = stepAction.Compile();
            var call = Expression.Call(Expression.Constant(this), _fakeExecuteActionMethod, stepAction.Body);
            var expression = Expression.Lambda<Action<TScenario>>(call, Expression.Parameter(typeof(TScenario)));
            AddStep(_ => compiledAction().Action(), expression, null, true, reports, executionOrder, asserts, stepPrefix);
        }

        [StepTitle("")]
#pragma warning disable CA1822 // Mark members as static
        private void ExecuteAction(ExampleAction _)
#pragma warning restore CA1822 // Mark members as static
        {
            
        }

        public void AddStep(
            Expression<Func<TScenario, Task>> stepAction, 
            string? stepTextTemplate, 
            bool? includeInputsInStepTitle, 
            bool reports, 
            ExecutionOrder executionOrder, 
            bool asserts, 
            string stepPrefix)
        {
            var action = stepAction.Compile();

            StepTitle title;
            List<StepArgument> args;

            if (string.IsNullOrWhiteSpace(stepTextTemplate) && IsChainedMethodCall(stepAction.Body))
            {
                title = FluentScanner<TScenario>.BuildChainedTitle(stepAction, stepPrefix);
                args = [];
            }
            else
            {
                var inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
                title = CreateTitle(stepTextTemplate, includeInputsInStepTitle, GetMethodInfo(stepAction), inputArguments, stepPrefix);
                args = [.. inputArguments.Where(s => !string.IsNullOrWhiteSpace(s.Name))];
            }

            var stepDelegate = StepActionFactory.GetStepAction(action);
            var shouldFixAsserts = FixAsserts(asserts, executionOrder);
            var shouldFixConsecutiveStep = FixConsecutiveStep(executionOrder);

            _steps.Add(new Step(stepDelegate, title, shouldFixAsserts, shouldFixConsecutiveStep, reports, args));
        }

        public void AddStep(
            Expression<Action<TScenario>> stepAction, 
            string? stepTextTemplate, 
            bool? includeInputsInStepTitle, 
            bool reports, 
            ExecutionOrder executionOrder, 
            bool asserts, 
            string stepPrefix)
        {
            var action = stepAction.Compile();

            AddStep(action, stepAction, stepTextTemplate, includeInputsInStepTitle, reports, executionOrder, asserts, stepPrefix);
        }

        private void AddStep(
            Action<TScenario> action, 
            LambdaExpression stepAction, 
            string? stepTextTemplate, 
            bool? includeInputsInStepTitle,
            bool reports, 
            ExecutionOrder executionOrder, 
            bool asserts, 
            string stepPrefix)
        {
            StepTitle title;
            List<StepArgument> args;

            if (string.IsNullOrWhiteSpace(stepTextTemplate) && IsChainedMethodCall(stepAction.Body))
            {
                title = FluentScanner<TScenario>.BuildChainedTitle(stepAction, stepPrefix);
                args = [];
            }
            else
            {
                var inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
                title = CreateTitle(stepTextTemplate, includeInputsInStepTitle, GetMethodInfo(stepAction), inputArguments, stepPrefix);
                args = [.. inputArguments.Where(s => !string.IsNullOrEmpty(s.Name))];
            }

            _steps.Add(new Step(StepActionFactory.GetStepAction(action), title, FixAsserts(asserts, executionOrder),
                FixConsecutiveStep(executionOrder), reports, args));
        }

        private StepTitle CreateTitle(
            string? stepTextTemplate,
            bool? includeInputsInStepTitle,
            MethodInfo methodInfo,
            StepArgument[] inputArguments,
            string stepPrefix) 
            => Configurator.StepTitleFactory.Create(
                stepTextTemplate, 
                includeInputsInStepTitle, 
                methodInfo, 
                inputArguments, 
                _testContext, 
                stepPrefix);

        private bool FixAsserts(bool asserts, ExecutionOrder executionOrder)
        {
            if (executionOrder == ExecutionOrder.ConsecutiveStep)
            {
                var lastStep = _steps.LastOrDefault();
                if (lastStep != null)
                {
                    return lastStep.Asserts;
                }
            }

            return asserts;
        }

        private ExecutionOrder FixConsecutiveStep(ExecutionOrder executionOrder)
        {
            if (executionOrder == ExecutionOrder.ConsecutiveStep)
            {
                var lastStep = _steps.LastOrDefault();
                if (lastStep != null)
                {
                    switch (lastStep.ExecutionOrder)
                    {
                        case ExecutionOrder.Initialize:
                        case ExecutionOrder.SetupState:
                        case ExecutionOrder.ConsecutiveSetupState:
                            return ExecutionOrder.ConsecutiveSetupState;
                        case ExecutionOrder.Transition:
                        case ExecutionOrder.ConsecutiveTransition:
                            return ExecutionOrder.ConsecutiveTransition;
                        case ExecutionOrder.Assertion:
                        case ExecutionOrder.ConsecutiveAssertion:
                        case ExecutionOrder.TearDown:
                            return ExecutionOrder.ConsecutiveAssertion;
                    }
                }
            }

            return executionOrder;
        }

        private static MethodInfo GetMethodInfo(LambdaExpression stepAction) => ((MethodCallExpression)stepAction.Body).Method;

        private static bool IsChainedMethodCall(Expression body) => body is MethodCallExpression { Object: MethodCallExpression };

        private static StepTitle BuildChainedTitle(LambdaExpression stepAction, string stepPrefix)
        {
            var chain = new List<(MethodInfo Method, StepArgument[] Args)>();
            var node = (MethodCallExpression)stepAction.Body;

            while (node is not null)
            {
                var args = FluentScanner<TScenario>.ExtractArgumentsFromCall(node);
                chain.Add((node.Method, args));
                node = node.Object as MethodCallExpression;
            }

            chain.Reverse();

            return new StepTitle(() =>
            {
                var parts = chain.Select(entry =>
                {
                    var humanized = Configurator.Humanizer.Humanize(entry.Method.Name);
                    if (entry.Args.Length > 0)
                    {
                        var argValues = entry.Args.Select(a => a.Value?.FlattenArray()).ToArray();
                        humanized = humanized + " " + string.Join(", ", argValues);
                    }
                    return humanized;
                });

                var title = string.Join(" ", parts).Trim();
                if (!string.IsNullOrEmpty(stepPrefix) && !title.StartsWith(stepPrefix, StringComparison.CurrentCultureIgnoreCase))
                {
                    title = $"{stepPrefix} {title[..1].ToLower()}{title[1..]}";
                }
                return title;
            });
        }

        private static StepArgument[] ExtractArgumentsFromCall(MethodCallExpression node)
        {
            var parameters = node.Method.GetParameters();
            var args = new StepArgument[node.Arguments.Count];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var arg = node.Arguments[i];
                var value = Expression.Lambda<Func<object>>(Expression.Convert(arg, typeof(object))).Compile();
                args[i] = new StepArgument(parameters[i].Name, parameters[i].ParameterType, value, null);
            }
            return args;
        }
    }
}
