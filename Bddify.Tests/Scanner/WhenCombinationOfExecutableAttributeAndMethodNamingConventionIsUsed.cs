using System;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Scanner
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
            public void ThenThisMethodIsFoundAsAnGivenStepNotThenStep()
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
                new ScanForScenarios(
                    new IScanForSteps[]
                        {
                            new DefaultScanForStepsByMethodName(), 
                            new ExecutableAttributeScanner()
                        }).Scan(typeof(ScenarioWithMixedSteps)).Single();          
        }

        [Test]
        public void GivenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.Given, ExecutionOrder.SetupState);
        }

        [Test]
        public void StepScannerPriorityIsConsidered()
        {
            VerifyStepAndItsProperties(_sut.ThenThisMethodIsFoundAsAnGivenStepNotThenStep, ExecutionOrder.ConsequentSetupState);
        }

        [Test]
        public void WhenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.When, ExecutionOrder.Transition);
        }

        [Test]
        public void LegacyTransitionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.LegacyTransitionMethod, ExecutionOrder.ConsequentTransition);
        }

        [Test]
        public void ThenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.Then, ExecutionOrder.Assertion);
        }

        [Test]
        public void AndThenStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.AndThen, ExecutionOrder.ConsequentAssertion);
        }

        [Test]
        public void LegacyAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.TestThatSomethingIsRight, ExecutionOrder.Assertion);
        }

        [Test]
        public void LegacyConsequentAssertionStepIsScanned()
        {
            VerifyStepAndItsProperties(_sut.TestThatSomethingIsWrong, ExecutionOrder.ConsequentAssertion);
        }

        void VerifyStepAndItsProperties(Action stepMethodAction, ExecutionOrder expectedOrder, int expectedCount = 1)
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(stepMethodAction));
            Assert.That(matchingSteps.Count(), Is.EqualTo(expectedCount));
            Assert.IsTrue(matchingSteps.All(s => s.ExecutionOrder == expectedOrder));
        }

        [Test]
        public void IgnoredMethodShouldNotBeAddedToSteps()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.ThenIAmNotAStep));
            Assert.That(matchingSteps.Count(), Is.EqualTo(0));
        }
    }
}