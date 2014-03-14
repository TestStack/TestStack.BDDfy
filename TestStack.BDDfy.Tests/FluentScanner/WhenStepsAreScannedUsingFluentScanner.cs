using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace TestStack.BDDfy.Tests.FluentScanner
{
    [TestFixture]
    public class WhenStepsAreScannedUsingFluentScanner
    {
        private IEnumerable<ExecutionStep> _steps;

        [SetUp]
        public void Setup()
        {
            var scenario = new ScenarioToBeScannedUsingFluentScanner();
            _steps = ScenarioToBeScannedUsingFluentScanner.GetSteps(scenario);
        }

        [Test]
        public void IndicatedStepsAreReturned()
        {
            Assert.That(_steps.Count(), Is.EqualTo(12));
        }

        ExecutionStep GivenSomeStateStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "Given some state 1, 2");
            }
        }

        [Test]
        public void GivenSomeState_StepIsASetupMethod()
        {
            Assert.That(GivenSomeStateStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
        }

        [Test]
        public void GivenSomeState_IsNotAsserting()
        {
            Assert.IsFalse(GivenSomeStateStep.Asserts);
        }

        [Test]
        public void GivenSomeState_StepReports()
        {
            Assert.IsTrue(GivenSomeStateStep.ShouldReport);        
        }

        ExecutionStep WhenSomeStepUsesIncompatibleNamingConventionStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "When some step uses incompatible naming convention");
            }
        }

        [Test]
        public void WhenSomeStepUsesIncompatibleNamingConvention_IsAConsecutiveSetupMethod()
        {
            Assert.That(WhenSomeStepUsesIncompatibleNamingConventionStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
        }

        [Test]
        public void WhenSomeStepUsesIncompatibleNamingConvention_DoesNotAssert()
        {
            Assert.IsFalse(WhenSomeStepUsesIncompatibleNamingConventionStep.Asserts);
        }

        [Test]
        public void WhenSomeStepUsesIncompatibleNamingConvention_Reports()
        {
            Assert.IsTrue(WhenSomeStepUsesIncompatibleNamingConventionStep.ShouldReport);        
        }

        ExecutionStep AndAMethodTakesArrayInputsStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "And a method takes array inputs 1, 2, 3, 4, 5");
            }
        }

        [Test]
        public void AndAMethodTakesArrayInputs_IsAConsecutiveSetupMethod()
        {
            Assert.That(AndAMethodTakesArrayInputsStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
        }

        [Test]
        public void AndAMethodTakesArrayInputs_DoesNotAssert()
        {
            Assert.IsFalse(AndAMethodTakesArrayInputsStep.Asserts);
        }

        [Test]
        public void AndAMethodTakesArrayInputs_Reports()
        {
            Assert.IsTrue(AndAMethodTakesArrayInputsStep.ShouldReport);
        }

        ExecutionStep WhenSomethingHappensTransitionStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "When something happens some input here");
            }
        }

        [Test]
        public void WhenSomethingHappensTransitionStep_IsATransitionStep()
        {
            Assert.That(WhenSomethingHappensTransitionStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Transition));
        }

        [Test]
        public void WhenSomethingHappensTransitionStep_DoesNotAssert()
        {
            Assert.IsFalse(WhenSomethingHappensTransitionStep.Asserts);
        }

        [Test]
        public void WhenSomethingHappensTransitionStep_Reports()
        {
            Assert.IsTrue(WhenSomethingHappensTransitionStep.ShouldReport);        
        }

        ExecutionStep WhenSomethingHappensTransitionStepIgnoringInputInStepTitle
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "When something happens");
            }
        }

        [Test]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_IsAConsecutiveTransitionStep()
        {
            Assert.That(WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
        }

        [Test]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_DoesNotAssert()
        {
            Assert.IsFalse(WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.Asserts);
        }

        [Test]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_Reports()
        {
            Assert.IsTrue(WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.ShouldReport);        
        }

        ExecutionStep WhenSomethingHappensConsecutiveTransitionStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "step used with other input for the second time");
            }
        }

        [Test]
        public void WhenSomethingHappensConsecutiveTransitionStep_IsAConsecutiveTransitionStep()
        {
            Assert.That(WhenSomethingHappensConsecutiveTransitionStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
        }

        [Test]
        public void WhenSomethingHappensConsecutiveTransitionStep_DoesNotAssert()
        {
            Assert.IsFalse(WhenSomethingHappensConsecutiveTransitionStep.Asserts);
        }

        [Test]
        public void WhenSomethingHappensConsecutiveTransitionStep_Reports()
        {
            Assert.IsTrue(WhenSomethingHappensConsecutiveTransitionStep.ShouldReport);        
        }

        ExecutionStep AndThenSomethingElseHappensStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "Overriding step name without arguments");
            }
        }

        [Test]
        public void AndThenSomethingElseHappensStep_IsAConsecutiveTransitionStep()
        {
            Assert.That(AndThenSomethingElseHappensStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
        }

        [Test]
        public void AndThenSomethingElseHappensStep_DoesNotAssert()
        {
            Assert.IsFalse(AndThenSomethingElseHappensStep.Asserts);
        }

        [Test]
        public void AndThenSomethingElseHappensStep_Reports()
        {
            Assert.IsTrue(AndThenSomethingElseHappensStep.ShouldReport);        
        }

        ExecutionStep ThenTheFollowingAssertionsShouldBeCorrectStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "Then the following assertions should be correct");
            }
        }

        [Test]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_IsAnAssertingStep()
        {
            Assert.That(ThenTheFollowingAssertionsShouldBeCorrectStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Assertion));
        }

        [Test]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_DoesAssert()
        {
            Assert.IsTrue(ThenTheFollowingAssertionsShouldBeCorrectStep.Asserts);
        }

        [Test]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_Reports()
        {
            Assert.IsTrue(ThenTheFollowingAssertionsShouldBeCorrectStep.ShouldReport);        
        }

        ExecutionStep AndIncorrectAttributeWouldNotMatterStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "And incorrect attribute would not matter");
            }
        }

        [Test]
        public void AndIncorrectAttributeWouldNotMatterStep_IsAConsecutiveAssertingStep()
        {
            Assert.That(AndIncorrectAttributeWouldNotMatterStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveAssertion));
        }

        [Test]
        public void AndIncorrectAttributeWouldNotMatterStep_DoesAssert()
        {
            Assert.IsTrue(AndIncorrectAttributeWouldNotMatterStep.Asserts);
        }

        [Test]
        public void AndIncorrectAttributeWouldNotMatterStep_Reports()
        {
            Assert.IsTrue(AndIncorrectAttributeWouldNotMatterStep.ShouldReport);        
        }

        ExecutionStep AndInputsAreFormattedPropertlyInTheTitle
        {
            get
            {
                var formattedTitle = string.Format(
                    ScenarioToBeScannedUsingFluentScanner.InputDateStepTitleTemplate, 
                    ScenarioToBeScannedUsingFluentScanner.InputDate);

                return _steps.Single(s => s.StepTitle.Trim() == formattedTitle);
            }
        }

        [Test]
        public void AndInputsAreFormattedPropertlyInTheTitle_IsAConsecutiveAssertingStep()
        {
            Assert.That(AndInputsAreFormattedPropertlyInTheTitle.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveAssertion));
        }

        ExecutionStep TearDownStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "Dispose");
            }
        }

        [Test]
        public void TearDownStep_IsAConsecutiveAssertingStep()
        {
            Assert.That(TearDownStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.TearDown));
        }

        [Test]
        public void TearDownStep_DoesAssert()
        {
            Assert.IsFalse(TearDownStep.Asserts);
        }

        [Test]
        public void TearDownStep_DoesNotReports()
        {
            Assert.IsFalse(TearDownStep.ShouldReport);        
        }
    }
}
