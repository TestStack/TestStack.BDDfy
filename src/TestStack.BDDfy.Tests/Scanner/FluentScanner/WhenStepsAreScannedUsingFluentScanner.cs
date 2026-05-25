using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class WhenStepsAreScannedUsingFluentScanner
    {
        private IEnumerable<Step> _steps;

        public WhenStepsAreScannedUsingFluentScanner()
        {
            var scenario = new ScenarioToBeScannedUsingFluentScanner();
            _steps = ScenarioToBeScannedUsingFluentScanner.GetSteps(scenario);
        }

        [Fact]
        public void IndicatedStepsAreReturned()
        {
            _steps.Count().ShouldBe(12);
        }

        Step GivenSomeStateStep
        {
            get
            {
                return _steps.Single(s => s.Title == "Given some state 1, 2");
            }
        }

        [Fact]
        public void GivenSomeState_StepIsASetupMethod()
        {
            GivenSomeStateStep.ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
        }

        [Fact]
        public void GivenSomeState_IsNotAsserting()
        {
            GivenSomeStateStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void GivenSomeState_StepReports()
        {
            GivenSomeStateStep.ShouldReport.ShouldBe(true);
        }

        Step WhenSomeStepUsesIncompatibleNamingConventionStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "And when some step uses incompatible naming convention");
            }
        }

        [Fact]
        public void WhenSomeStepUsesIncompatibleNamingConvention_IsAConsecutiveSetupMethod()
        {
            WhenSomeStepUsesIncompatibleNamingConventionStep.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveSetupState);
        }

        [Fact]
        public void WhenSomeStepUsesIncompatibleNamingConvention_DoesNotAssert()
        {
            WhenSomeStepUsesIncompatibleNamingConventionStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void WhenSomeStepUsesIncompatibleNamingConvention_Reports()
        {
            WhenSomeStepUsesIncompatibleNamingConventionStep.ShouldReport.ShouldBe(true);
        }

        Step AndAMethodTakesArrayInputsStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "And a method takes array inputs 1, 2, 3, 4, 5");
            }
        }

        [Fact]
        public void AndAMethodTakesArrayInputs_IsAConsecutiveSetupMethod()
        {
            AndAMethodTakesArrayInputsStep.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveSetupState);
        }

        [Fact]
        public void AndAMethodTakesArrayInputs_DoesNotAssert()
        {
            AndAMethodTakesArrayInputsStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void AndAMethodTakesArrayInputs_Reports()
        {
            AndAMethodTakesArrayInputsStep.ShouldReport.ShouldBe(true);
        }

        Step WhenSomethingHappensTransitionStep
        {
            get
            {
                return _steps.Single(s => s.Title == "When something happens some input here");
            }
        }

        [Fact]
        public void WhenSomethingHappensTransitionStep_IsATransitionStep()
        {
            WhenSomethingHappensTransitionStep.ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
        }

        [Fact]
        public void WhenSomethingHappensTransitionStep_DoesNotAssert()
        {
            WhenSomethingHappensTransitionStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void WhenSomethingHappensTransitionStep_Reports()
        {
            WhenSomethingHappensTransitionStep.ShouldReport.ShouldBe(true);
        }

        Step WhenSomethingHappensTransitionStepIgnoringInputInStepTitle
        {
            get
            {
                return _steps.Single(s => s.Title == "And when something happens");
            }
        }

        [Fact]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_IsAConsecutiveTransitionStep()
        {
            WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveTransition);
        }

        [Fact]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_DoesNotAssert()
        {
            WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.Asserts.ShouldBe(false);
        }

        [Fact]
        public void WhenSomethingHappensTransitionStepIgnoringInputInStepTitle_Reports()
        {
            WhenSomethingHappensTransitionStepIgnoringInputInStepTitle.ShouldReport.ShouldBe(true);
        }

        Step WhenSomethingHappensConsecutiveTransitionStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "step used with other input for the second time");
            }
        }

        [Fact]
        public void WhenSomethingHappensConsecutiveTransitionStep_IsAConsecutiveTransitionStep()
        {
            WhenSomethingHappensConsecutiveTransitionStep.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveTransition);
        }

        [Fact]
        public void WhenSomethingHappensConsecutiveTransitionStep_DoesNotAssert()
        {
            WhenSomethingHappensConsecutiveTransitionStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void WhenSomethingHappensConsecutiveTransitionStep_Reports()
        {
            WhenSomethingHappensConsecutiveTransitionStep.ShouldReport.ShouldBe(true);
        }

        Step AndThenSomethingElseHappensStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "Overriding step name without arguments");
            }
        }

        [Fact]
        public void AndThenSomethingElseHappensStep_IsAConsecutiveTransitionStep()
        {
            AndThenSomethingElseHappensStep.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveTransition);
        }

        [Fact]
        public void AndThenSomethingElseHappensStep_DoesNotAssert()
        {
            AndThenSomethingElseHappensStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void AndThenSomethingElseHappensStep_Reports()
        {
            AndThenSomethingElseHappensStep.ShouldReport.ShouldBe(true);
        }

        Step ThenTheFollowingAssertionsShouldBeCorrectStep
        {
            get
            {
                return _steps.Single(s => s.Title == "Then the following assertions should be correct");
            }
        }

        [Fact]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_IsAnAssertingStep()
        {
            ThenTheFollowingAssertionsShouldBeCorrectStep.ExecutionOrder.ShouldBe(ExecutionOrder.Assertion);
        }

        [Fact]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_DoesAssert()
        {
            ThenTheFollowingAssertionsShouldBeCorrectStep.Asserts.ShouldBe(true);
        }

        [Fact]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_Reports()
        {
            ThenTheFollowingAssertionsShouldBeCorrectStep.ShouldReport.ShouldBe(true);
        }

        Step AndIncorrectAttributeWouldNotMatterStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "And incorrect attribute would not matter");
            }
        }

        [Fact]
        public void AndIncorrectAttributeWouldNotMatterStep_IsAConsecutiveAssertingStep()
        {
            AndIncorrectAttributeWouldNotMatterStep.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
        }

        [Fact]
        public void AndIncorrectAttributeWouldNotMatterStep_DoesAssert()
        {
            AndIncorrectAttributeWouldNotMatterStep.Asserts.ShouldBe(true);
        }

        [Fact]
        public void AndIncorrectAttributeWouldNotMatterStep_Reports()
        {
            AndIncorrectAttributeWouldNotMatterStep.ShouldReport.ShouldBe(true);
        }

        Step AndInputsAreFormattedPropertlyInTheTitle
        {
            get
            {
                var formattedTitle = string.Format(
                    ScenarioToBeScannedUsingFluentScanner.InputDateStepTitleTemplate, 
                    ScenarioToBeScannedUsingFluentScanner.InputDate);

                return _steps.Single(s => s.Title.Trim() == formattedTitle);
            }
        }

        [Fact]
        public void AndInputsAreFormattedPropertlyInTheTitle_IsAConsecutiveAssertingStep()
        {
            AndInputsAreFormattedPropertlyInTheTitle.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
        }

        Step TearDownStep
        {
            get
            {
                return _steps.Single(s => s.Title == "Dispose");
            }
        }

        [Fact]
        public void TearDownStep_IsAConsecutiveAssertingStep()
        {
            TearDownStep.ExecutionOrder.ShouldBe(ExecutionOrder.TearDown);
        }

        [Fact]
        public void TearDownStep_DoesAssert()
        {
            TearDownStep.Asserts.ShouldBe(false);
        }

        [Fact]
        public void TearDownStep_DoesNotReports()
        {
            TearDownStep.ShouldReport.ShouldBe(false);
        }
    }
}
