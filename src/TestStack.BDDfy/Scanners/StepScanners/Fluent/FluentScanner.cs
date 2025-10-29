using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TestStack.BDDfy.Annotations;
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
    /// [Fact]
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
        private readonly List<Step> _steps = new();
        private readonly TScenario _testObject;
        private readonly ITestContext _testContext;
        private readonly MethodInfo _fakeExecuteActionMethod;

        internal FluentScanner(TScenario testObject)
        {
            _testObject = testObject;
            _testContext = TestContext.GetContext(_testObject);
            _fakeExecuteActionMethod = typeof(FluentScanner<TScenario>).GetMethod("ExecuteAction", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        IScanner IFluentScanner.GetScanner(string scenarioTitle, Type explicitStoryType)
        {
            return new DefaultScanner(_testContext, new FluentScenarioScanner(_steps, scenarioTitle), explicitStoryType);
        }

        public void AddStep(Action stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var action = StepActionFactory.GetStepAction<object>(o => stepAction());
            var stepTitle = CreateTitle(title, stepPrefix);
            _steps.Add(new Step(action, stepTitle, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports, new List<StepArgument>()));
        }

        public void AddStep(Func<Task> stepAction, string title, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var action = StepActionFactory.GetStepAction<object>(o => stepAction());
            var stepTitle = CreateTitle(title, stepPrefix);
            _steps.Add(new Step(action, stepTitle, FixAsserts(asserts, executionOrder), FixConsecutiveStep(executionOrder), reports, new List<StepArgument>()));
        }

        private StepTitle CreateTitle(string title, string stepPrefix) => Configurator.StepTitleFactory.Create(title, stepPrefix, _testContext);

        public void AddStep(Expression<Func<ExampleAction>> stepAction, bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var compiledAction = stepAction.Compile();
            var call = Expression.Call(Expression.Constant(this), _fakeExecuteActionMethod, stepAction.Body);
            var expression = Expression.Lambda<Action<TScenario>>(call, Expression.Parameter(typeof(TScenario)));
            AddStep(_ => compiledAction().Action(), expression, null, true, reports, executionOrder, asserts, stepPrefix);
        }

        [UsedImplicitly]
        [StepTitle("")]
        private void ExecuteAction(ExampleAction action)
        {
            
        }

        public void AddStep(
            Expression<Func<TScenario, Task>> stepAction, 
            string stepTextTemplate, 
            bool includeInputsInStepTitle, 
            bool reports, 
            ExecutionOrder executionOrder, 
            bool asserts, 
            string stepPrefix)
        {
            var action = stepAction.Compile();
            var inputArguments = Array.Empty<StepArgument>();
            if (includeInputsInStepTitle)
            {
                inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
            }

            var title = CreateTitle(
                stepTextTemplate, 
                includeInputsInStepTitle, 
                GetMethodInfo(stepAction), 
                inputArguments,
                stepPrefix);

            var args = inputArguments.Where(s => !string.IsNullOrEmpty(s.Name)).ToList();
            var stepDelegate = StepActionFactory.GetStepAction(action);
            var shouldFixAsserts = FixAsserts(asserts, executionOrder);
            var shouldFixConsecutiveStep = FixConsecutiveStep(executionOrder);

            _steps.Add(new Step(stepDelegate, title, shouldFixAsserts, shouldFixConsecutiveStep, reports, args));
        }

        public void AddStep(
            Expression<Action<TScenario>> stepAction, 
            string stepTextTemplate, 
            bool includeInputsInStepTitle, 
            bool reports, 
            ExecutionOrder executionOrder, 
            bool asserts, 
            string stepPrefix)
        {
            var action = stepAction.Compile();

            AddStep(action, stepAction, stepTextTemplate, includeInputsInStepTitle, reports, executionOrder, asserts, stepPrefix);
        }

        private void AddStep(Action<TScenario> action, LambdaExpression stepAction, string stepTextTemplate, bool includeInputsInStepTitle,
            bool reports, ExecutionOrder executionOrder, bool asserts, string stepPrefix)
        {
            var inputArguments = Array.Empty<StepArgument>();
            if (includeInputsInStepTitle)
            {
                inputArguments = stepAction.ExtractArguments(_testObject).ToArray();
            }

            var title = CreateTitle(stepTextTemplate, includeInputsInStepTitle, GetMethodInfo(stepAction), inputArguments, stepPrefix);

            var args = inputArguments.Where(s => !string.IsNullOrEmpty(s.Name)).ToList();
            _steps.Add(new Step(StepActionFactory.GetStepAction(action), title, FixAsserts(asserts, executionOrder),
                FixConsecutiveStep(executionOrder), reports, args));
        }

        private StepTitle CreateTitle(
            string stepTextTemplate,
            bool includeInputsInStepTitle,
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

        private static MethodInfo GetMethodInfo(LambdaExpression stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }
    }
}
