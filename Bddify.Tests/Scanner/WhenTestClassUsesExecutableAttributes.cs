using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace Bddify.Tests.Scanner
{
    public class WhenTestClassUsesExecutableAttributes
    {
        private TypeWithAttribute _typeWithAttribute;
        private List<ExecutionStep> _steps;

        private class TypeWithAttribute
        {
            public const string MethodTextForWhenSomethingHappens = "When Something Happens";
            public const string MethodTextForAndThen = "The text for the and then part";

            [Then]
            [RunStepWithArgs(1, 2)]
            [RunStepWithArgs(3, 4)]
            public void Then(int input1, int input2) { }

            public void ThenIShouldNotBeReturnedBecauseIDoNotHaveAttributes() { }

            [When(StepText = MethodTextForWhenSomethingHappens)]
            public void When() { }

            public void WhenNoAttributeIsProvided() { }

            [Given]
            public void Given() { }

            public void GivenWithoutAttribute() { }

            [AndWhen]
            public void TheOtherPartOfWhen() { }

            [AndThen(StepText = MethodTextForAndThen)]
            public void AndThen() { }

            [AndGiven]
            public void SomeOtherPartOfTheGiven() { }
        }

        [SetUp]
        public void WhenTestClassHasAttributes()
        {
            _typeWithAttribute = new TypeWithAttribute();
            _steps = new ExecutableAttributeScanner().Scan(typeof(TypeWithAttribute)).ToList();
        }

        private static string GetStepTextFromMethodName(Action methodInfoAction)
        {
            return NetToString.Convert(Helpers.GetMethodInfo(methodInfoAction).Name);
        }

        [Test]
        public void DecoratedMethodsAreReturned()
        {
            Assert.That(_steps.Count, Is.EqualTo(7));
        }

        IEnumerable<ExecutionStep> GetStepsByMethodInfo(MethodInfo stepMethod)
        {
            return _steps.Where(s => s.Method == stepMethod);
        }

        ExecutionStep GetStepByMethodInfo(MethodInfo stepMethod)
        {
            return _steps.Single(s => s.Method == stepMethod);
        }

        ExecutionStep GivenStep
        {
            get
            {
                return GetStepByMethodInfo(Helpers.GetMethodInfo(_typeWithAttribute.Given));
            }
        }

        [Test]
        public void GivenStepIsFoundAsSetupState()
        {
            Assert.That(GivenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
        }

        [Test]
        public void GivenStepDoesNotAssert()
        {
            Assert.IsFalse(GivenStep.Asserts);
        }

        [Test]
        public void GivenStepTextIsFetchedFromMethodName()
        {
            Assert.That(GivenStep.ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.Given)));
        }

        ExecutionStep AndGivenStep
        {
            get
            {
                return GetStepByMethodInfo(Helpers.GetMethodInfo(_typeWithAttribute.SomeOtherPartOfTheGiven));
            }
        }

        [Test]
        public void AndGivenIsFoundAsConsequtiveSetupState()
        {
            Assert.That(AndGivenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentSetupState));
        }

        [Test]
        public void AndGivenStepDoesNotAssert()
        {
            Assert.IsFalse(AndGivenStep.Asserts);
        }

        [Test]
        public void AndGivenStepTextIsFetchedFromMethodName()
        {
            Assert.That(AndGivenStep.ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        ExecutionStep WhenStep
        {
            get
            {
                return GetStepByMethodInfo(Helpers.GetMethodInfo(_typeWithAttribute.When));
            }
        }

        [Test]
        public void WhenIsFoundAsTransitionStep()
        {
            Assert.That(WhenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.Transition));
        }

        [Test]
        public void WhenStepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(WhenStep.ReadableMethodName, Is.EqualTo(TypeWithAttribute.MethodTextForWhenSomethingHappens));
        }

        [Test]
        public void WhenStepDoesNotAssert()
        {
            Assert.IsFalse(WhenStep.Asserts);
        }

        ExecutionStep AndWhenStep
        {
            get
            {
                return GetStepByMethodInfo(Helpers.GetMethodInfo(_typeWithAttribute.TheOtherPartOfWhen));
            }
        }

        [Test]
        public void AndWhenIsFoundAsConsequentTransition()
        {
            Assert.That(AndWhenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentTransition));
        }

        [Test]
        public void AndWhenStepTextIsFetchedFromMethodName()
        {
            Assert.That(AndWhenStep.ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen)));
        }

        [Test]
        public void AndWhenStepDoesNotAssert()
        {
            Assert.IsFalse(AndWhenStep.Asserts);
        }

        [Test]
        public void ThenStepsAreReturnedAsAssertingSteps()
        {
            var steps = ThenSteps.ToList();
            Assert.IsTrue(steps.All(s => s.ExecutionOrder == ExecutionOrder.Assertion));
        }

        [Test]
        public void ThenStepsAreProvidedWithCorrectArguments()
        {
            var steps = ThenSteps.ToList();
            Assert.IsTrue(steps.All(s => s.InputArguments != null && s.InputArguments.Length == 2));
            Assert.IsTrue(steps.All(s => s.InputArguments.SequenceEqual(new object[] {1, 2}) || s.InputArguments.SequenceEqual(new object[] {3, 4})));
        }

        IEnumerable<ExecutionStep> ThenSteps
        {
            get
            {
                var methodInfo = _typeWithAttribute.GetType().GetMethod("Then", new[] { typeof(int), typeof(int) });
                return GetStepsByMethodInfo(methodInfo);
            }
        }

        [Test]
        public void ThenStepsDoAssert()
        {
            var steps = ThenSteps.ToList();
            Assert.True(steps.All(s => s.Asserts));
        }

        [Test]        
        public void ThenStepTextIsFetchedFromMethodNamePostfixedWithArguments()
        {
            var steps = ThenSteps.ToList();
            Assert.IsTrue(steps.All(s => s.ReadableMethodName.EndsWith(" 1, 2") || s.ReadableMethodName.EndsWith(" 3, 4")));
        }

        ExecutionStep AndThenStep
        {
            get
            {
                return GetStepByMethodInfo(Helpers.GetMethodInfo(_typeWithAttribute.AndThen));
            }
        }

        [Test]
        public void AndThenIsFoundAndConsequentAssertingStep()
        {
            Assert.That(AndThenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsequentAssertion));
        }

        [Test]
        public void AndThenStepAsserts()
        {
            Assert.IsTrue(AndThenStep.Asserts);
        }

        [Test]
        public void AndThenStepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(AndThenStep.ReadableMethodName, Is.EqualTo(TypeWithAttribute.MethodTextForAndThen));
        }
    }
}