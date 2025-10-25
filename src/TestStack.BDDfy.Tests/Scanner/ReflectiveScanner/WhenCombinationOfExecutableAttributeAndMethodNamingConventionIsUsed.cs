using System;
using System.Linq;
using System.Linq.Expressions;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenCombinationOfExecutableAttributeAndMethodNamingConventionIsUsed
    {
        private Scenario _scenario;
        private ScenarioWithMixedSteps _sut;

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
                    new IStepScanner[]
                        {
                            new ExecutableAttributeStepScanner(),
                            new DefaultMethodNameStepScanner()
                        }).Scan(TestContext.GetContext(_sut)).First();
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
            VerifyStepAndItsProperties(() => _sut.ThenThisMethodIsFoundAsAGivenStepNotThenStep(), ExecutionOrder.ConsecutiveSetupState);
        }

        [Fact]
        public void WhenStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.When(), ExecutionOrder.Transition);
        }

        [Fact]
        public void LegacyTransitionStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.LegacyTransitionMethod(), ExecutionOrder.ConsecutiveTransition);
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
            VerifyStepAndItsProperties(() => _sut.TestThatSomethingIsRight(), ExecutionOrder.Assertion);
        }

        [Fact]
        public void LegacyConsecutiveAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(() => _sut.TestThatSomethingIsWrong(), ExecutionOrder.ConsecutiveAssertion);
        }

        void VerifyStepAndItsProperties(Expression<Action> stepMethodAction, ExecutionOrder expectedOrder, int expectedCount = 1)
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Title.Trim() == Configurator.Humanizer.Humanize(Helpers.GetMethodInfo(stepMethodAction).Name));
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