using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
    public class WhenTestClassUsesExecutableAttributes
    {
        private TypeWithAttribute _typeWithAttribute;
        private List<Step> _steps;

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
            _steps = new ExecutableAttributeStepScanner().Scan(_typeWithAttribute.Given("")).ToList();
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

        Step GivenStep
        {
            get
            {
                return _steps.Single(s => s.Title == "Given");
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
            Assert.That(GivenStep.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.Given)));
        }

        Step AndGivenStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "Some other part of the given");
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
            Assert.That(AndGivenStep.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        Step WhenStep
        {
            get
            {
                return _steps.Single(s => s.Title == TypeWithAttribute.MethodTextForWhenSomethingHappens);
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
            Assert.That(WhenStep.Title, Is.EqualTo(TypeWithAttribute.MethodTextForWhenSomethingHappens));
        }

        [Test]
        public void WhenStep_StepDoesNotAssert()
        {
            Assert.IsFalse(WhenStep.Asserts);
        }

        Step TheOtherPartOfWhenStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == "The other part of when");
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
            Assert.That(TheOtherPartOfWhenStep.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen)));
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

        IEnumerable<Step> ThenSteps
        {
            get
            {
                return _steps.Where(s => s.Title == "Then 1, 2" || s.Title == "Then 3, 4");
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
            Assert.IsTrue(steps.All(s => s.Title.EndsWith(" 1, 2") || s.Title.EndsWith(" 3, 4")));
        }

        Step AndThenStep
        {
            get
            {
                return _steps.Single(s => s.Title.Trim() == TypeWithAttribute.MethodTextForAndThen);
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
            Assert.That(AndThenStep.Title.Trim(), Is.EqualTo(TypeWithAttribute.MethodTextForAndThen));
        }
    }
}