using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
    public class WhenTestClassUsesExecutableAttributes
    {
        private TypeWithAttribute _typeWithAttribute;
        private List<ExecutionStep> _steps;

        private class TypeWithAttribute
        {
            public const string MethodTextForWhenSomethingHappens = "When Something Happens";
            public const string MethodTextForAndThen = "The text for the AndThen part";

            [Then]
            [RunStepWithArgs(1, 2)]
            [RunStepWithArgs(3, 4)]
            public void Then(int input1, int input2) { }

            public void ThenIShouldNotBeReturnedBecauseIDoNotHaveAttributes() { }

            [When(StepTitle = MethodTextForWhenSomethingHappens)]
            public void WhenStep() { }

            public void WhenStep_NoAttributeIsProvided() { }

            [Given]
            public void Given() { }

            public void GivenWithoutAttribute() { }

            [AndWhen]
            public void TheOtherPartOfWhen() { }

            [AndThen(StepTitle = MethodTextForAndThen)]
            public void AndThen() { }

            [AndGiven]
            public void SomeOtherPartOfTheGiven() { }
        }

        [SetUp]
        public void WhenStep_TestClassHasAttributes()
        {
            _typeWithAttribute = new TypeWithAttribute();
            _steps = new ExecutableAttributeStepScanner().Scan(_typeWithAttribute).ToList();
        }

        private static string GetStepTextFromMethodName(Action methodInfoAction)
        {
            return Configurator.Scanners.Humanize(Helpers.GetMethodInfo(methodInfoAction).Name);
        }

        [Test]
        public void DecoratedMethodsAreReturned()
        {
            Assert.That(_steps.Count, Is.EqualTo(7));
        }

        ExecutionStep GivenStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == "Given");
            }
        }

        [Test]
        public void GivenStep_IsFoundAsSetupState()
        {
            Assert.That(GivenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
        }

        [Test]
        public void GivenStep_DoesNotAssert()
        {
            Assert.IsFalse(GivenStep.Asserts);
        }

        [Test]
        public void GivenStep_TextIsFetchedFromMethodName()
        {
            Assert.That(GivenStep.StepTitle.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.Given)));
        }

        ExecutionStep AndGivenStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "Some other part of the given");
            }
        }

        [Test]
        public void AndGivenIsFoundAsConsequtiveSetupState()
        {
            Assert.That(AndGivenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
        }

        [Test]
        public void AndGivenStepDoesNotAssert()
        {
            Assert.IsFalse(AndGivenStep.Asserts);
        }

        [Test]
        public void AndGivenStepTextIsFetchedFromMethodName()
        {
            Assert.That(AndGivenStep.StepTitle.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        ExecutionStep WhenStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle == TypeWithAttribute.MethodTextForWhenSomethingHappens);
            }
        }

        [Test]
        public void WhenStep_IsFoundAsTransitionStep()
        {
            Assert.That(WhenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Transition));
        }

        [Test]
        public void WhenStep_StepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(WhenStep.StepTitle, Is.EqualTo(TypeWithAttribute.MethodTextForWhenSomethingHappens));
        }

        [Test]
        public void WhenStep_StepDoesNotAssert()
        {
            Assert.IsFalse(WhenStep.Asserts);
        }

        ExecutionStep TheOtherPartOfWhenStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == "The other part of when");
            }
        }

        [Test]
        public void TheOtherPartOfWhenStep_IsFoundAsConsecutiveTransition()
        {
            Assert.That(TheOtherPartOfWhenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
        }

        [Test]
        public void TheOtherPartOfWhenStep_StepTextIsFetchedFromMethodName()
        {
            Assert.That(TheOtherPartOfWhenStep.StepTitle.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen)));
        }

        [Test]
        public void TheOtherPartOfWhenStep_StepDoesNotAssert()
        {
            Assert.IsFalse(TheOtherPartOfWhenStep.Asserts);
        }

        [Test]
        public void ThenStepsAreReturnedAsAssertingSteps()
        {
            var steps = ThenSteps.ToList();
            Assert.IsTrue(steps.All(s => s.ExecutionOrder == ExecutionOrder.Assertion));
        }

        IEnumerable<ExecutionStep> ThenSteps
        {
            get
            {
                return _steps.Where(s => s.StepTitle == "Then 1, 2" || s.StepTitle == "Then 3, 4");
            }
        }

        [Test]
        public void ThenSteps_DoAssert()
        {
            var steps = ThenSteps.ToList();
            Assert.True(steps.All(s => s.Asserts));
        }

        [Test]        
        public void ThenSteps_StepTextIsFetchedFromMethodNamePostfixedWithArguments()
        {
            var steps = ThenSteps.ToList();
            Assert.IsTrue(steps.All(s => s.StepTitle.EndsWith(" 1, 2") || s.StepTitle.EndsWith(" 3, 4")));
        }

        ExecutionStep AndThenStep
        {
            get
            {
                return _steps.Single(s => s.StepTitle.Trim() == TypeWithAttribute.MethodTextForAndThen);
            }
        }

        [Test]
        public void AndThenStep_IsFoundAndConsecutiveAssertingStep()
        {
            Assert.That(AndThenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveAssertion));
        }

        [Test]
        public void AndThenStep_Asserts()
        {
            Assert.IsTrue(AndThenStep.Asserts);
        }

        [Test]
        public void AndThenStep_StepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(AndThenStep.StepTitle.Trim(), Is.EqualTo(TypeWithAttribute.MethodTextForAndThen));
        }
    }
}