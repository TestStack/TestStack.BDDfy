using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Provides an alternative method of implementing stories and scenarios.
    /// </summary>
    /// <remarks>
    /// Reflecting scanners run in a pipeline which means you can mix and match their
    /// usage in your scenario; however, when you use FluentStepScanner, BDDfy does not
    /// use other scanners which means method names and attributes are ignored for
    /// scanning methods. You are in full control of what steps you want
    /// run and in what order.
    /// </remarks>
    /// <typeparam name="TScenario"></typeparam>
    /// <example>
    /// <code>
    /// [Test]
    /// public void AccountHasSufficientFund()
    /// {
    ///     this.Given(s => s.GivenTheAccountBalanceIs(100), GivenTheAccountBalanceIsTitleTemplate)
    ///             .And(s => s.AndTheCardIsValid())
    ///             .And(s => s.AndTheMachineContains(100), AndTheMachineContainsEnoughMoneyTitleTemplate)
    ///         .When(s => s.WhenTheAccountHolderRequests(20), WhenTheAccountHolderRequestsTitleTemplate)
    ///         .Then(s => s.TheAtmShouldDispense(20), "Then the ATM should dispense $20")
    ///             .And(s => s.AndTheAccountBalanceShouldBe(80), "And the account balance should be $80")
    ///             .And(s => s.ThenCardIsRetained(false), AndTheCardShouldBeReturnedTitleTemplate)
    ///         .BDDfy(storyCategory: "ATM");
    /// }
    /// </code>
    /// </example>
    internal class FluentScanner<TScenario> : IFluentScanner
        where TScenario : class
    {
        private readonly List<Step> _steps = new List<Step>();
        private readonly TScenario _testObject;
        private readonly ITestContext _testContext;
        private readonly List<StepArgument> _arguments = new List<StepArgument>();

        internal FluentScanner(TScenario testObject)
        {
            _testObject = testObject;
            _testContext = TestContext.GetContext(_testObject);
        }

        IScanner IFluentScanner.GetScanner(string scenarioTitle, Type explicitStoryType)
        {
            return new DefaultScanner(_testContext, new FluentScenarioScanner(_steps, scenarioTitle, _arguments), explicitStoryType);
        }

        public void AddStep(Action stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts)
        {
            _steps.Add(new Step(StepActionFactory.GetStepAction<object>(o => stepAction()), new StepTitle(title), FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports));
        }

        public void AddStep(Func<Task> stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts)
        {
            _steps.Add(new Step(StepActionFactory.GetStepAction<object>(o => stepAction()), new StepTitle(title), FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports));
        }

        public void AddStep(Expression<Func<TScenario, Task>> stepAction, string stepTextTemplate, bool includeInputsInStepTitle, bool reports, ExecutionOrder executionOrder, bool asserts)
        {
            var action = stepAction.Compile();
            var inputArguments = new StepArgument[0];
            if (includeInputsInStepTitle)
            {
                inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
            }

            var title = CreateTitle(stepTextTemplate, includeInputsInStepTitle, GetMethodInfo(stepAction), inputArguments);
            _arguments.AddRange(inputArguments.Where(s => !string.IsNullOrEmpty(s.Name)));
            _steps.Add(new Step(StepActionFactory.GetStepAction(action), title, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports));
        }

        public void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool includeInputsInStepTitle, bool reports, ExecutionOrder executionOrder, bool asserts)
        {
            var action = stepAction.Compile();

            var inputArguments = new StepArgument[0];
            if (includeInputsInStepTitle)
            {
                inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
            }

            var title = CreateTitle(stepTextTemplate, includeInputsInStepTitle, GetMethodInfo(stepAction), inputArguments);
            _arguments.AddRange(inputArguments.Where(s => !string.IsNullOrEmpty(s.Name)));
            _steps.Add(new Step(StepActionFactory.GetStepAction(action), title, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports));
        }

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

        private StepTitle CreateTitle(string stepTextTemplate, bool includeInputsInStepTitle, MethodInfo methodInfo, StepArgument[] inputArguments)
        {
            Func<string> createTitle = () =>
                {

                    var flatInputArray = inputArguments.Select(o => o.Value).FlattenArrays();
                    var stepTitle = Configurator.Scanners.Humanize(methodInfo.Name);

                    if (!string.IsNullOrEmpty(stepTextTemplate)) stepTitle = string.Format(stepTextTemplate, flatInputArray);
                    else if (includeInputsInStepTitle)
                    {
                        var parameters = methodInfo.GetParameters();
                        var stringFlatInputs =
                            flatInputArray
                                .Select((a, i) => new { ParameterName = parameters[i].Name, Value = a })
                                .Select(i =>
                                {
                                    if (_testContext.Examples != null)
                                    {

                                        var matchingHeader = _testContext.Examples.Headers.SingleOrDefault(header => ExampleTable.HeaderMatches(header, i.ParameterName));
                                        if (matchingHeader != null)
                                            return string.Format("<{0}>", matchingHeader);
                                    }
                                    return i.Value.ToString();
                                })
                                .ToArray();
                        stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
                    }

                    return stepTitle.Trim();
                };

            return new StepTitle(createTitle);
        }

        private static MethodInfo GetMethodInfo(Expression<Func<TScenario, Task>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }

        private static MethodInfo GetMethodInfo(Expression<Action<TScenario>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }
    }
}
