using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

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
    internal class FluentScanner<TScenario> : IInitialStep<TScenario>, IGiven<TScenario>, IWhen<TScenario>, IThen<TScenario>
        where TScenario : class
    {
        private readonly List<Step> _steps = new List<Step>();
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
   
        IGiven<TScenario> IInitialStep<TScenario>.Given(Expression<Func<TScenario, Task>> givenStep, string stepTextTemplate)
        {
            AddStep(givenStep, stepTextTemplate, false, ExecutionOrder.SetupState);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(Expression<Func<TScenario, Task>> whenStep, string stepTextTemplate)
        {
            AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
            return this;
        }

        IGiven<TScenario> IInitialStep<TScenario>.Given(Expression<Func<TScenario, Task>> givenStep, bool includeInputsInStepTitle)
        {
            AddStep(givenStep, null, false, ExecutionOrder.SetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(Expression<Func<TScenario, Task>> whenStep, bool includeInputsInStepTitle)
        {
            AddStep(whenStep, null, false, ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(Expression<Func<TScenario, Task>> thenStep, string stepTextTemplate)
        {
            AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(Expression<Func<TScenario, Task>> andThenStep, string stepTextTemplate)
        {
            AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(Expression<Func<TScenario, Task>> andThenStep, bool includeInputsInStepTitle)
        {
            AddStep(andThenStep, null, true, ExecutionOrder.ConsecutiveAssertion, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(Expression<Func<TScenario, Task>> andWhenStep, bool includeInputsInStepTitle)
        {
            AddStep(andWhenStep, null, false, ExecutionOrder.ConsecutiveTransition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(Expression<Func<TScenario, Task>> thenStep, bool includeInputsInStepTitle)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(Expression<Func<TScenario, Task>> andGivenStep, bool includeInputsInStepTitle)
        {
            AddStep(andGivenStep, null, false, ExecutionOrder.ConsecutiveSetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(Expression<Func<TScenario, Task>> andGivenStep, string stepTextTemplate)
        {
            AddStep(andGivenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(Expression<Func<TScenario, Task>> andWhenStep, string stepTextTemplate)
        {
            AddStep(andWhenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IStepsBase<TScenario> IStepsBase<TScenario>.TearDownWith(Expression<Func<TScenario, Task>> tearDownStep)
        {
            AddStep(tearDownStep, null, false, ExecutionOrder.TearDown, false);
            return this;
        }

        private void AddStep(Expression<Func<TScenario, Task>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder, bool reports = true, bool includeInputsInStepTitle = true)
        {
            var methodInfo = GetMethodInfo(stepAction);
            var inputArguments = new object[0];
            if (includeInputsInStepTitle)
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
            _steps.Add(new Step(StepActionFactory.GetStepAction(action), stepTitle, asserts, executionOrder, reports));
        }

        private static MethodInfo GetMethodInfo(Expression<Func<TScenario, Task>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }

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
            _steps.Add(new Step(StepActionFactory.GetStepAction(action), stepTitle, asserts, executionOrder, reports));
        }

        private void AddStep(Action stepAction, string title, bool asserts, ExecutionOrder executionOrder, bool reports = true)
        {
            _steps.Add(new Step(o => stepAction(), title, asserts, executionOrder, reports));
        }

        public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null)
        {
            AddStep(givenStep, stepTextTemplate, false, ExecutionOrder.SetupState);
            return this;
        }

        public IGiven<TScenario> Given(Action givenStep, string title)
        {
            AddStep(givenStep, title, false, ExecutionOrder.SetupState);
            return this;
        }

        public IGiven<TScenario> Given(string title)
        {
            AddStep(() => { }, title, false, ExecutionOrder.SetupState);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
        {
            AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(Action whenStep, string title)
        {
            AddStep(whenStep, title, false, ExecutionOrder.Transition);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(string title)
        {
            AddStep(() => { }, title, false, ExecutionOrder.Transition);
            return this;
        }

        IGiven<TScenario> IInitialStep<TScenario>.Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
        {
            AddStep(givenStep, null, false, ExecutionOrder.SetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IWhen<TScenario> IWhenSteps<TScenario>.When(Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
        {
            AddStep(whenStep, null, false,ExecutionOrder.Transition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, bool includeInputsInStepTitle)
        {
            AddStep(andGivenStep, null, false, ExecutionOrder.ConsecutiveSetupState, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle)
        {
            AddStep(thenStep, null, true, ExecutionOrder.Assertion, includeInputsInStepTitle:includeInputsInStepTitle);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle)
        {
            AddStep(andWhenStep, null, false, ExecutionOrder.ConsecutiveTransition, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate)
        {
            AddStep(andGivenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(Action andGivenStep, string title)
        {
            AddStep(andGivenStep, title, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IGiven<TScenario> IGiven<TScenario>.And(string title)
        {
            AddStep(() => {}, title, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, bool includeInputsInStepTitle)
        {
            AddStep(andThenStep, null, true, ExecutionOrder.ConsecutiveAssertion, includeInputsInStepTitle: includeInputsInStepTitle);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(Action thenStep, string title)
        {
            AddStep(thenStep, title, true, ExecutionOrder.Assertion);
            return this;
        }

        IThen<TScenario> IThenSteps<TScenario>.Then(string title)
        {
            AddStep(() => { }, title, true, ExecutionOrder.Assertion);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate)
        {
            AddStep(andWhenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate)
        {
            AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(Action andWhenStep, string title)
        {
            AddStep(andWhenStep, title, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IWhen<TScenario> IWhen<TScenario>.And(string title)
        {
            AddStep(() => { }, title, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(Action andThenStep, string title)
        {
            AddStep(andThenStep, title, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IThen<TScenario> IThen<TScenario>.And(string title)
        {
            AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IStepsBase<TScenario> IStepsBase<TScenario>.TearDownWith(Expression<Action<TScenario>> tearDownStep)
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
