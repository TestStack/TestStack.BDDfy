using System.Collections.Generic;
using Bddify.Core;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.FluentStepScanner
{
    public class WhenStepsAreScannedUsingFluentStepScanner
    {
        private IEnumerable<ExecutionStep> _steps;
        private readonly ScenarioToBeScannedUsingFluentScanner _dummyInstance = new ScenarioToBeScannedUsingFluentScanner();

        [SetUp]
        public void Setup()
        {
            var scanner = ScenarioToBeScannedUsingFluentScanner.GetScanner();
            _steps = scanner.Scan(typeof(ScenarioToBeScannedUsingFluentScanner));
        }

        [Test]
        public void IndicatedStepsAreReturned()
        {
            Assert.That(_steps.Count(), Is.EqualTo(10));
        }

        ExecutionStep GivenSomeStateStep
        {
            get
            {
                return _steps.Single(s => s.Method.Name == "GivenSomeState");
            }
        }

        [Test]
        public void GivenSomeState_StepHasTheCorrectArguments()
        {
            Assert.True(GivenSomeStateStep.InputArguments.SequenceEqual(new object[] {1, 2}));
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

        [Test]
        public void GivenSomeState_StepHasCorrectReadableMethodName()
        {
            Assert.That(GivenSomeStateStep.ReadableMethodName, Is.EqualTo("Given some state 1, 2"));
        }

        ExecutionStep WhenSomeStateUsesIncompatibleNamingConventionStep
        {
            get
            {
                return _steps.Single(s => s.Method == Helpers.GetMethodInfo(_dummyInstance.WhenSomeStateUsesIncompatibleNamingConvention));
            }
        }

        [Test]
        public void WhenSomeStateUsesIncompatibleNamingConvention_HasTheCorrectArguments()
        {
            Assert.IsEmpty(WhenSomeStateUsesIncompatibleNamingConventionStep.InputArguments);
        }

        [Test]
        public void WhenSomeStateUsesIncompatibleNamingConvention_IsAConsecutiveSetupMethod()
        {
            Assert.That(WhenSomeStateUsesIncompatibleNamingConventionStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
        }

        [Test]
        public void WhenSomeStateUsesIncompatibleNamingConvention_DoesNotAssert()
        {
            Assert.IsFalse(WhenSomeStateUsesIncompatibleNamingConventionStep.Asserts);
        }

        [Test]
        public void WhenSomeStateUsesIncompatibleNamingConvention_Reports()
        {
            Assert.IsTrue(WhenSomeStateUsesIncompatibleNamingConventionStep.ShouldReport);        
        }

        [Test]
        public void WhenSomeStateUsesIncompatibleNamingConvention_HasCorrectReadableMethodName()
        {
            Assert.That(WhenSomeStateUsesIncompatibleNamingConventionStep.ReadableMethodName.Trim(), Is.EqualTo("When some state uses incompatible naming convention"));
        }

        ExecutionStep AndAMethodTakesArrayInputsStep
        {
            get
            {
                return _steps.Single(s => s.Method.Name == "AndAMethodTakesArrayInputs");
            }
        }

        [Test]
        public void AndAMethodTakesArrayInputs_StringArrayArgumentsAreSet()
        {
            var stringArrayInputs = AndAMethodTakesArrayInputsStep.InputArguments[0] as string[];
            Assert.That(stringArrayInputs, Is.Not.Null);
            Assert.AreEqual(stringArrayInputs[0], "1");
            Assert.AreEqual(stringArrayInputs[1], "2");
        }

        [Test]
        public void AndAMethodTakesArrayInputs_IntArrayArgumentsAreSet()
        {
            var intArrayInputs = AndAMethodTakesArrayInputsStep.InputArguments[1] as int[];
            Assert.That(intArrayInputs, Is.Not.Null);
            Assert.AreEqual(intArrayInputs[0], 3);
            Assert.AreEqual(intArrayInputs[1], 4);
        }

        [Test]
        public void AndAMethodTakesArrayInputs_IntArgumentIsSet()
        {
            var intInput = (int)AndAMethodTakesArrayInputsStep.InputArguments[2];
            Assert.AreEqual(intInput, 5);
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

        [Test]
        public void AndAMethodTakesArrayInputs_HasCorrectReadableMethodName()
        {
            Assert.That(AndAMethodTakesArrayInputsStep.ReadableMethodName.Trim(), Is.EqualTo("And a method takes array inputs 1, 2, 3, 4, 5"));
        }


        ExecutionStep WhenSomethingHappensTransitionStep
        {
            get
            {
                return _steps.Single(s => s.Method.Name == "WhenSomethingHappens" && s.InputArguments.Count() == 1 && (string) s.InputArguments[0] == "some input here");
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

        [Test]
        public void WhenSomethingHappensTransitionStep_HasCorrectReadableMethodName()
        {
            Assert.That(WhenSomethingHappensTransitionStep.ReadableMethodName.Trim(), Is.EqualTo("When something happens some input here"));
        }

        ExecutionStep WhenSomethingHappensConsecutiveTransitionStep
        {
            get
            {
                return _steps.Single(s => s.Method.Name == "WhenSomethingHappens" && s.InputArguments.Count() == 1 && (string) s.InputArguments[0] == "other input");
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

        [Test]
        public void WhenSomethingHappensConsecutiveTransitionStep_HasCorrectReadableMethodName()
        {
            Assert.That(WhenSomethingHappensConsecutiveTransitionStep.ReadableMethodName.Trim(), Is.EqualTo("step used with other input for the second time"));
        }

        ExecutionStep AndThenSomethingElseHappensStep
        {
            get
            {
                return _steps.Single(s => s.Method == Helpers.GetMethodInfo(_dummyInstance.AndThenSomethingElseHappens));
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

        [Test]
        public void AndThenSomethingElseHappensStep_HasCorrectReadableMethodName()
        {
            Assert.That(AndThenSomethingElseHappensStep.ReadableMethodName.Trim(), Is.EqualTo("Overriding step name without arguments"));
        }

        ExecutionStep ThenTheFollowingAssertionsShouldBeCorrectStep
        {
            get
            {
                return _steps.Single(s => s.Method == Helpers.GetMethodInfo(_dummyInstance.ThenTheFollowingAssertionsShouldBeCorrect));
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

        [Test]
        public void ThenTheFollowingAssertionsShouldBeCorrectStep_HasCorrectReadableMethodName()
        {
            Assert.That(ThenTheFollowingAssertionsShouldBeCorrectStep.ReadableMethodName.Trim(), Is.EqualTo("Then the following assertions should be correct"));
        }

        ExecutionStep AndIncorrectAttributeWouldNotMatterStep
        {
            get
            {
                return _steps.Single(s => s.Method == Helpers.GetMethodInfo(_dummyInstance.AndIncorrectAttributeWouldNotMatter));
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

        [Test]
        public void AndIncorrectAttributeWouldNotMatterStep_HasCorrectReadableMethodName()
        {
            Assert.That(AndIncorrectAttributeWouldNotMatterStep.ReadableMethodName.Trim(), Is.EqualTo("And incorrect attribute would not matter"));
        }

        ExecutionStep TearDownStep
        {
            get
            {
                return _steps.Single(s => s.Method == Helpers.GetMethodInfo(_dummyInstance.Dispose));
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
