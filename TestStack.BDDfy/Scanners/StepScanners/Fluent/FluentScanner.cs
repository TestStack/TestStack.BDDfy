using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.ScenarioScanners;

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
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
    internal class FluentScanner<TScenario> : IInitialStep<TScenario>, IAndGiven<TScenario>, IAndWhen<TScenario>, IAndThen<TScenario>
        where TScenario : class
    {
        private readonly List<ExecutionStep> _steps = new List<ExecutionStep>();
        private readonly object _testObject;

        object IHasScanner.TestObject
        {
            get { return _testObject; }
        }

        internal FluentScanner(object testObject)
        {
            _testObject = testObject;
        }

        IScanner IHasScanner.GetScanner(string scenarioTitle, Type explicitStoryType)
        {
            return new DefaultScanner(_testObject, new FluentScenarioScanner(_steps, scenarioTitle), explicitStoryType);
        }
#if NET35
        public void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder)
        {
            AddStep(stepAction, stepTextTemplate, asserts, executionOrder, true);
        }

        public IWhen<TScenario> When(Expression<Action<TScenario>> whenStep)
        {
            AddStep(whenStep, null, false, ExecutionOrder.Transition);
            return this;
        }

        public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep)
        {
            return Given(givenStep, null);
        }

        IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep)
        {
            return When(whenStep);
        }

        IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion);
            return this;
        }

        IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion);
            return this;
        }

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep)
        {
            return ((IThen<TScenario>)this).And(andThenStep, null);
        }

        IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep)
        {
            return ((IWhen<TScenario>)this).And(andWhenStep, null);
        }

        IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep)
        {
            return ((IGiven<TScenario>)this).And(andGivenStep, null);
        }
#endif

        private void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder, bool reports = true, bool includeInputsInStepTitle = true)
        {
            var methodInfo = GetMethodInfo(stepAction);
            var inputArguments = new object[0];
            if(includeInputsInStepTitle)
            {
                inputArguments = stepAction.ExtractConstants().ToArray();
            }

            var flatInputArray = inputArguments.FlattenArrays();
            var stepTitle = NetToString.Convert(methodInfo.Name);

            if (!string.IsNullOrEmpty(stepTextTemplate))
                stepTitle = string.Format(stepTextTemplate, flatInputArray);
            else if (includeInputsInStepTitle)
            {
                var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
                stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            }

            stepTitle = stepTitle.Trim();
            var action = stepAction.Compile();
            _steps.Add(new ExecutionStep(StepActionFactory.GetStepAction(action), stepTitle, asserts, executionOrder, reports));
        }

        public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null)
        {
            AddStep(givenStep, stepTextTemplate, false, ExecutionOrder.SetupState);
            return this;
        }

        IWhen<TScenario> IInitialStep<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
        {
            AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
            return this;
        }

        IGiven<TScenario> IInitialStep<TScenario>.Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
        {
            AddStep(givenStep, null, false, ExecutionOrder.SetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IWhen<TScenario> IInitialStep<TScenario>.When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
        {
            AddStep(whenStep, null, false,ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle)
        {
            AddStep(andGivenStep, null, false, ExecutionOrder.ConsecutiveSetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle:includeInputsInStepTitle);
            return this;
        }

        IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle)
        {
            AddStep(andWhenStep, null, false, ExecutionOrder.ConsecutiveTransition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle:includeInputsInStepTitle);
            return this;
        }

        IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate)
        {
            AddStep(andGivenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle)
        {
            AddStep(andThenStep, null, true, ExecutionOrder.ConsecutiveAssertion, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
            return this;
        }

        IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
        {
            AddStep(whenStep, null, false, ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate)
        {
            AddStep(andWhenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
            return this;
        }

        IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
        {
            AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
            return this;
        }

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate)
        {
            AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IFluentScanner<TScenario> IFluentScanner<TScenario>.TearDownWith(Expression<Action<TScenario>> tearDownStep)
        {
            AddStep(tearDownStep, null, false, ExecutionOrder.TearDown, false);
            return this;
        }

        private static MethodInfo GetMethodInfo(Expression<Action<TScenario>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }
    }
}