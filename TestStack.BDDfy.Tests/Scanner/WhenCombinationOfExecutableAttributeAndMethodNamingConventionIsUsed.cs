using System;
using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
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

        [SetUp]
        public void Setup()
        {
            _sut = new ScenarioWithMixedSteps();
            _scenario = 
                new ReflectiveScenarioScanner(
                    new IStepScanner[]
                        {
                            new ExecutableAttributeStepScanner(),
                            new DefaultMethodNameStepScanner()
                        }).Scan(_sut);          
        }

        [Test]
        public void ScenarioTextIsSetUsingClassName()
        {
            Assert.That(_scenario.Title, Is.EqualTo("Scenario with mixed steps"));    
        }

        [Test]
        public void GivenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.Given, ExecutionOrder.SetupState);
        }

        [Test]
        public void ExecutableAttributesHaveHigherPriorityThanNamingConventions()
        {
            VerifyStepAndItsProperties(_sut.ThenThisMethodIsFoundAsAGivenStepNotThenStep, ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void WhenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.When, ExecutionOrder.Transition);
        }

        [Test]
        public void LegacyTransitionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.LegacyTransitionMethod, ExecutionOrder.ConsecutiveTransition);
        }

        [Test]
        public void ThenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.Then, ExecutionOrder.Assertion);
        }

        [Test]
        public void AndThenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.AndThen, ExecutionOrder.ConsecutiveAssertion);
        }

        [Test]
        public void LegacyAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.TestThatSomethingIsRight, ExecutionOrder.Assertion);
        }

        [Test]
        public void LegacyConsecutiveAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.TestThatSomethingIsWrong, ExecutionOrder.ConsecutiveAssertion);
        }

        void VerifyStepAndItsProperties(Action stepMethodAction, ExecutionOrder expectedOrder, int expectedCount = 1)
        {
            var matchingSteps = _scenario.Steps.Where(s => s.StepTitle.Trim() == Configurator.Scanners.Humanize(Helpers.GetMethodInfo(stepMethodAction).Name));
            Assert.That(matchingSteps.Count(), Is.EqualTo(expectedCount));
            Assert.IsTrue(matchingSteps.All(s => s.ExecutionOrder == expectedOrder));
        }

        [Test]
        public void IgnoredMethodShouldNotBeAddedToSteps()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.StepTitle == Configurator.Scanners.Humanize(Helpers.GetMethodInfo(_sut.ThenIAmNotAStep).Name));
            Assert.That(matchingSteps.Count(), Is.EqualTo(0));
        }
    }
}