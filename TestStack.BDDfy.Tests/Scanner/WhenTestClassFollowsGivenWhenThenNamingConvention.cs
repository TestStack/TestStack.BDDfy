using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TestStack.BDDfy.Tests.Scanner
{
    // ToDo: I really need to clean this class up
    [TestFixture]
    public class WhenTestClassFollowsGivenWhenThenNamingConvention
    {
        private List<Step> _steps;
        private TypeWithoutAttribute _typeWithoutAttribute;

        private class TypeWithoutAttribute
        {
            public void EstablishContext() {}
            public void Setup(){} 
            public void AndThenAnotherThingIsTrue() {}
            public void AndWhenSomethingElseHappens() {}
            public void And_When_another_THING_Happens() {}
            public void GivenSomeState() {}
            public void AndSomethingElseToo() { }
            public void WhenSomethingHappens() {}
            public void AndGivenAnotherState() { }
            public void And_Given_Some_OTHER_state() { }
            public void AndGiven_some_other_initial_state() { }
            public void ThenSomethingIsTrue(){}
            public void TearDown(){}
            public void And_YET_another_thing(){}
            public void AndThen_something_else() {}
            public void And_then_there_was_that_one_time() {}
        }

        [SetUp]
        public void Setup()
        {
            _typeWithoutAttribute = new TypeWithoutAttribute();
            _steps = new DefaultMethodNameStepScanner().Scan(new TestContext(_typeWithoutAttribute)).ToList();
        }
            
        [Test]
        public void AllMethodsFollowingTheNamingConventionAreReturnedAsSteps()
        {
            Assert.That(_steps.Count, Is.EqualTo(16));
        }

        private static void AssertStep(Step step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            Assert.That(step.Title.Trim(), Is.EqualTo(stepTitle));
            Assert.That(step.Asserts, Is.EqualTo(asserts));
            Assert.That(step.ExecutionOrder, Is.EqualTo(order));
            Assert.That(step.ShouldReport, Is.EqualTo(shouldReport));
        }

        [Test]
        public void EndsWithContext_IsReturnedFirst()
        {
            AssertStep(_steps[0], "Establish context", ExecutionOrder.Initialize, false, false);
        }

        [Test]
        public void Setup_IsReturnedSecond()
        {
            AssertStep(_steps[1], "Setup", ExecutionOrder.Initialize, false, false);
        }

        [Test]
        public void Given_IsTurnedIntoA_Given_Step()
        {
            AssertStep(_steps[2], "Given some state", ExecutionOrder.SetupState);
        }

        [Test]
        public void AndGiven_IsTurnedIntoAn_AndGiven_Step()
        {
            AssertStep(_steps[3], "And another state", ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void And_Given_IsTurnedIntoAn_AndGiven_Step()
        {
            AssertStep(_steps[4], "And Some OTHER state", ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void AndGiven_InAnUnderscoredMethod_IsTurnedIntoAn_AndGiven_Step()
        {
            AssertStep(_steps[5], "And some other initial state", ExecutionOrder.ConsecutiveSetupState);
        }

        [Test]
        public void WhenIsReturnedAfterGivens()
        {
            AssertStep(_steps[6], "When something happens", ExecutionOrder.Transition);
        }

        [Test]
        public void AndWhenIsTurnedIntoAn_AndWhen_Step()
        {
            AssertStep(_steps[7], "And something else happens", ExecutionOrder.ConsecutiveTransition);
        }

        [Test]
        public void And_When_IsTurnedIntoAn_AndWhen_Step()
        {
            AssertStep(_steps[8], "And another THING Happens", ExecutionOrder.ConsecutiveTransition);
        }

        [Test]
        public void ThenIsReturnedAfterWhens()
        {
            AssertStep(_steps[9], "Then something is true", ExecutionOrder.Assertion, true);
        }

        [Test]
        public void AndThen_IsReturnedAsAn_AndThen_StepAfterThen()
        {
            AssertStep(_steps[10], "And another thing is true", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void And_IsReturnedAsAn_AndThen_Step()
        {
            AssertStep(_steps[11], "And something else too", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void And_IsReturnedAsAn_AndThen_WithTheRightCasing()
        {
            AssertStep(_steps[12], "And YET another thing", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void AndThen_IsReturnedAsAn_AndThen_WithUnderscoredMethodName()
        {
            AssertStep(_steps[13], "And something else", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void And_Then_IsReturnedAsAn_AndThen_WithUnderscoredMethodName()
        {
            AssertStep(_steps[14], "And there was that one time", ExecutionOrder.ConsecutiveAssertion, true);
        }

        [Test]
        public void TearDownMethodIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[15], "Tear down", ExecutionOrder.TearDown, asserts:false, shouldReport:false);
        }
    }
}