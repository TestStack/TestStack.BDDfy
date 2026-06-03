using System.Linq.Expressions;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenCombinationOfExecutableAttributeAndMethodNamingConventionIsUsed
    {
        private readonly Scenario _scenario;
        private readonly ScenarioWithMixedSteps _sut;

        private class ScenarioWithMixedSteps
        {
            public void When()
            {
            }

            [AndWhen]
            public void LegacyTransitionMethod()
            {
                
            }

            [AndGiven]
            public void ThenThisMethodIsFoundAsAGivenStepNotThenStep()
            {}

            public void Then()
            {
            }

            public void Given()
            {
            }

            [AndThen]
            public void AndThen()
            {
            }

            [Then]
            public void TestThatSomethingIsRight()
            {
            }

            [AndThen]
            public void TestThatSomethingIsWrong()
            {
            }

            [IgnoreStep]
            public void ThenIAmNotAStep()
            {
            }
        }

        public WhenCombinationOfExecutableAttributeAndMethodNamingConventionIsUsed()
        {
            _sut = new ScenarioWithMixedSteps();
            _scenario =
                new ReflectiveScenarioScanner(
                    [
                      new ExecutableAttributeStepScanner(),
                      new DefaultMethodNameStepScanner()
                    ]).Scan(TestContext.GetContext(_sut)).First();
        }

        [Fact]
        public void ScenarioTextIsSetUsingClassName()
        {
            _scenario.Title.ShouldBe("Scenario with mixed steps");
        }

        [Fact]
        public void GivenStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.Given(), ExecutionOrder.SetupState);
        }

        [Fact]
        public void ExecutableAttributesHaveHigherPriorityThanNamingConventions()
        {
            VerifyStepAndItsProperties(() => _sut.ThenThisMethodIsFoundAsAGivenStepNotThenStep(), ExecutionOrder.ConsecutiveSetupState, expectedTitle: "And this method is found as a given step not then step");
        }

        [Fact]
        public void WhenStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.When(), ExecutionOrder.Transition);
        }

        [Fact]
        public void LegacyTransitionStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.LegacyTransitionMethod(), ExecutionOrder.ConsecutiveTransition, expectedTitle: "And legacy transition method");
        }

        [Fact]
        public void ThenStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.Then(), ExecutionOrder.Assertion);
        }

        [Fact]
        public void AndThenStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.AndThen(), ExecutionOrder.ConsecutiveAssertion);
        }

        [Fact]
        public void LegacyAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.TestThatSomethingIsRight(), ExecutionOrder.ConsecutiveAssertion, expectedTitle: "And test that something is right");
        }

        [Fact]
        public void LegacyConsecutiveAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.TestThatSomethingIsWrong(), ExecutionOrder.ConsecutiveAssertion, expectedTitle: "And test that something is wrong");
        }

        void VerifyStepAndItsProperties(Expression<Action> stepMethodAction, ExecutionOrder expectedOrder, int expectedCount = 1, string? expectedTitle = null)
        {
            var title = expectedTitle ?? Configurator.Humanizer.Humanize(Helpers.GetMethodInfo(stepMethodAction).Name);
            var matchingSteps = _scenario.Steps.Where(s => s.Title == title);
            matchingSteps.Count().ShouldBe(expectedCount);
            matchingSteps.All(s => s.ExecutionOrder == expectedOrder).ShouldBe(true);
        }

        [Fact]
        public void IgnoredMethodShouldNotBeAddedToSteps()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Title == Configurator.Humanizer.Humanize(Helpers.GetMethodInfo(() => _sut.ThenIAmNotAStep()).Name));
            matchingSteps.ShouldBeEmpty();
        }
    }
}