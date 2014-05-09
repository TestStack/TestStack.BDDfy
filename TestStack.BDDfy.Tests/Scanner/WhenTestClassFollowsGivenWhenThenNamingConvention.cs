using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TestStack.BDDfy.Tests.Scanner
{
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
            public void But_some_condition_is_not_true() {}
            public void ButSomeOtherConditionIsNotTrue() {}
        }

        [Test]
        public void VerifyScannedSteps()
        {
            _typeWithoutAttribute = new TypeWithoutAttribute();
            _steps = new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(_typeWithoutAttribute)).ToList();
            int stepIndex = 0;
        
            Assert.That(_steps.Count, Is.EqualTo(18));
            AssertStep(_steps[stepIndex++], "Establish context", ExecutionOrder.Initialize, false, false);
            AssertStep(_steps[stepIndex++], "Setup", ExecutionOrder.Initialize, false, false);
            AssertStep(_steps[stepIndex++], "Given some state", ExecutionOrder.SetupState);
            AssertStep(_steps[stepIndex++], "And another state", ExecutionOrder.ConsecutiveSetupState);
            AssertStep(_steps[stepIndex++], "And Some OTHER state", ExecutionOrder.ConsecutiveSetupState);
            AssertStep(_steps[stepIndex++], "And some other initial state", ExecutionOrder.ConsecutiveSetupState);
            AssertStep(_steps[stepIndex++], "When something happens", ExecutionOrder.Transition);
            AssertStep(_steps[stepIndex++], "And something else happens", ExecutionOrder.ConsecutiveTransition);
            AssertStep(_steps[stepIndex++], "And another THING Happens", ExecutionOrder.ConsecutiveTransition);
            AssertStep(_steps[stepIndex++], "Then something is true", ExecutionOrder.Assertion, true);
            AssertStep(_steps[stepIndex++], "And another thing is true", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "And something else too", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "And YET another thing", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "And something else", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "And there was that one time", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "But some condition is not true", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "But some other condition is not true", ExecutionOrder.ConsecutiveAssertion, true);
            AssertStep(_steps[stepIndex++], "Tear down", ExecutionOrder.TearDown, asserts:false, shouldReport:false);
        }

        private static void AssertStep(Step step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            Assert.That(step.Title.Trim(), Is.EqualTo(stepTitle));
            Assert.That(step.Asserts, Is.EqualTo(asserts));
            Assert.That(step.ExecutionOrder, Is.EqualTo(order));
            Assert.That(step.ShouldReport, Is.EqualTo(shouldReport));
        }
    }
}