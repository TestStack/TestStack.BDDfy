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
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.Given));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var givenStep = matchingSteps.First();
            Assert.That(givenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
        }

        [Test]
        public void WhenStepIsScanned()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.When));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var whenStep = matchingSteps.First();
            Assert.That(whenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Transition));
        }

        [Test]
        public void LegacyTransitionStepIsScanned()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.LegacyTransitionMethod));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var transitionStep = matchingSteps.First();
            Assert.That(transitionStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentTransition));
        }

        [Test]
        public void ThenStepIsScanned()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.Then));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var thenStep = matchingSteps.First();
            Assert.That(thenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Assertion));
        }

        [Test]
        public void AndThenStepIsScanned() // this also means that a method is considered only once even if it matches more than one scanner
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.AndThen));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var andThenStep = matchingSteps.First();
            Assert.That(andThenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentAssertion));
        }

        [Test]
        public void LegacyAssertionStepIsScanned()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.TestThatSomethingIsRight));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var testStep = matchingSteps.First();
            Assert.That(testStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Assertion));
        }

        [Test]
        public void LegacyConsequentAssertionStepIsScanned()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.TestThatSomethingIsWrong));
            Assert.That(matchingSteps.Count(), Is.EqualTo(1));
            var testStep = matchingSteps.First();
            Assert.That(testStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentAssertion));
        }

        [Test]
        public void IgnoredMethodShouldNotBeAddedToSteps()
        {
            var matchingSteps = _scenario.Steps.Where(s => s.Method == Helpers.GetMethodInfo(_sut.ThenIAmNotAStep));
            Assert.That(matchingSteps.Count(), Is.EqualTo(0));
        }
    }
}