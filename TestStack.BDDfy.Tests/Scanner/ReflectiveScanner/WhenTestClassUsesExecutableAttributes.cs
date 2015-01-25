using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
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

            public void ButWhenSomethingDoesNotHappen() { }

            [ButWhen]
            public void AndSomethingHasNotHappened() { }
            public void WhenStep_NoAttributeIsProvided() { }

            [Given]
            public void Given() { }

            public void GivenWithoutAttribute() { }
            
            [ButGiven]
            public void SetupShouldAvoidSomethings() { }

            public void ButGivenSomethingIsNotSetup() {}

            [AndWhen]
            public void TheOtherPartOfWhen() { }

            [AndThen(StepTitle = MethodTextForAndThen)]
            public void AndThen() { }

            [AndGiven]
            public void SomeOtherPartOfTheGiven() { }

            [But]
            public void IDontWantThisToBeTrue() { }

            [Executable(ExecutionOrder.Assertion, "", ShouldReport = false)]
            public void Executable() { }
        }

        [SetUp]
        public void WhenStep_TestClassHasAttributes()
        {
            _typeWithAttribute = new TypeWithAttribute();
            _steps = new ExecutableAttributeStepScanner().Scan(TestContext.GetContext(_typeWithAttribute)).ToList();
        }

        private static string GetStepTextFromMethodName(Action methodInfoAction)
        {
            return Configurator.Scanners.Humanize(Helpers.GetMethodInfo(methodInfoAction).Name);
        }

        [Test]
        public void DecoratedMethodsAreReturned()
        {
            Assert.That(_steps.Count, Is.EqualTo(11));
        }

        [Test]
        public void Given()
        {
            var givenStep = _steps.Single(s => s.Title == "Given"); 
            Assert.That(givenStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
            Assert.IsFalse(givenStep.Asserts);
            Assert.That(givenStep.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.Given)));
        }

        [Test]
        public void AndGiven()
        {
            var step = _steps.Single(s => s.Title.Trim() == "Some other part of the given");
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
            Assert.IsFalse(step.Asserts);
            Assert.That(step.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        [Test]
        public void ButGiven()
        {
            var step = _steps.Single(s => s.Title.Trim() == "Setup should avoid somethings");
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveSetupState));
            Assert.IsFalse(step.Asserts);
            Assert.That(step.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SetupShouldAvoidSomethings)));
        }

        [Test]
        public void When()
        {
            var step = _steps.Single(s => s.Title == TypeWithAttribute.MethodTextForWhenSomethingHappens);
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.Transition));
            Assert.That(step.Title, Is.EqualTo(TypeWithAttribute.MethodTextForWhenSomethingHappens));
            Assert.IsFalse(step.Asserts);
        }

        [Test]
        public void TheOtherPartOfWhen()
        {
            var step = _steps.Single(s => s.Title.Trim() == "The other part of when");
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
            Assert.That(step.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen)));
            Assert.IsFalse(step.Asserts);
        }

        [Test]
        public void ButWhen()
        {
            var step = _steps.Single(s => s.Title.Trim() == "And something has not happened");
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveTransition));
            Assert.That(step.Title.Trim(), Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.AndSomethingHasNotHappened)));
            Assert.IsFalse(step.Asserts);
        }

        [Test]
        public void ThenStepsWithArgs()
        {
            var steps = _steps.Where(s => s.Title == "Then 1, 2" || s.Title == "Then 3, 4").ToList();
            Assert.IsTrue(steps.All(s => s.ExecutionOrder == ExecutionOrder.Assertion));
            Assert.IsTrue(steps.All(s => s.Asserts));
            Assert.IsTrue(steps.All(s => s.Title.EndsWith(" 1, 2") || s.Title.EndsWith(" 3, 4")));
        }

        [Test]
        public void AndThen()
        {
            var step = _steps.Single(s => s.Title.Trim() == TypeWithAttribute.MethodTextForAndThen);
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveAssertion));
            Assert.IsTrue(step.Asserts);
            Assert.That(step.Title.Trim(), Is.EqualTo(TypeWithAttribute.MethodTextForAndThen));
        }

        [Test]
        public void But()
        {
            var step = _steps.Single(s => s.Title == "I dont want this to be true");
            Assert.That(step.ExecutionOrder, Is.EqualTo(ExecutionOrder.ConsecutiveAssertion));
            Assert.IsTrue(step.Asserts);
        }

        [Test]
        public void ExecutableAttributesDefaultToShouldReport()
        {
            foreach (var step in _steps.Where(s => s.Title != "Executable"))
            {
                Assert.IsTrue(step.ShouldReport);
            }
        }

        [Test]
        public void CanPreventExecutableAttributesReporting()
        {
            var step = _steps.First(s => s.Title == "Executable");
            Assert.IsFalse(step.ShouldReport);
        }
    }
}