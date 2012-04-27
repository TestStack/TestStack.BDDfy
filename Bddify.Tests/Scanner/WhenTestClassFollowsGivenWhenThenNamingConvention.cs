using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners.MethodName;
using NUnit.Framework;
using System.Linq;
using Bddify.Scanners.ScenarioScanners;

namespace Bddify.Tests.Scanner
{
    [TestFixture]
    public class WhenTestClassFollowsGivenWhenThenNamingConvention
    {
        private List<ExecutionStep> _steps;
        private TypeWithoutAttribute _typeWithoutAttribute;

        private class TypeWithoutAttribute
        {
            public void AndThen() {}
            public void AndWhen1() {}
            public void AndWhen2() {}
            public void Given() {}
            public void AndSomething() { }
            public void When() {}
            public void AndGiven1() { }
            public void AndGiven2() { }
            public void Then(){}
            public void TearDown(){}
        }

        [SetUp]
        public void Setup()
        {
            _typeWithoutAttribute = new TypeWithoutAttribute();
            _steps = new DefaultMethodNameStepScanner().Scan(_typeWithoutAttribute).ToList();
        }
            
        [Test]
        public void AllMethodsFollowingTheNamingConventionAreReturnedAsSteps()
        {
            Assert.That(_steps.Count, Is.EqualTo(10));
        }

        private static void AssertStep(ExecutionStep step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            Assert.That(step.StepTitle.Trim(), Is.EqualTo(stepTitle));
            Assert.That(step.Asserts, Is.EqualTo(asserts));
            Assert.That(step.ExecutionOrder, Is.EqualTo(order));
            Assert.That(step.ShouldReport, Is.EqualTo(shouldReport));
        }

        [Test]
        public void GivenIsReturnedFirst()
        {
            AssertStep(_steps[0], "Given", ExecutionOrder.SetupState);
        }

        [Test]
        public void AndGiven1IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[1], "And 1", ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void AndGiven2IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[2], "And 2", ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void WhenIsReturnedAfterGivens()
        {
            AssertStep(_steps[3], "When", ExecutionOrder.Transition);
        }

        [Test]
        public void AndWhen1IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[4], "And 1", ExecutionOrder.ConsecutiveTransition);
        }

        [Test]
        public void AndWhen2IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[5], "And 2", ExecutionOrder.ConsecutiveTransition);
        }

        [Test]
        public void ThenIsReturnedAfterWhens()
        {
            AssertStep(_steps[6], "Then", ExecutionOrder.Assertion, true);
        }

        [Test]
        public void AndThenIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[7], "And then", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void AndSomethingIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[8], "And something", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void TearDownMethodIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[9], "Tear down", ExecutionOrder.TearDown, asserts:false, shouldReport:false);
        }
    }
}