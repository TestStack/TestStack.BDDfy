using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
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

        public WhenTestClassUsesExecutableAttributes()
        {
            _typeWithAttribute = new TypeWithAttribute();
            _steps = new ExecutableAttributeStepScanner().Scan(TestContext.GetContext(_typeWithAttribute)).ToList();
        }

        private static string GetStepTextFromMethodName(Action methodInfoAction)
        {
            return Configurator.Scanners.Humanize(Helpers.GetMethodInfo(methodInfoAction).Name);
        }

        [Fact]
        public void DecoratedMethodsAreReturned()
        {
            _steps.Count.ShouldBe(11);
        }

        [Fact]
        public void Given()
        {
            var givenStep = _steps.Single(s => s.Title == "Given"); 
            givenStep.ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
            givenStep.Asserts.ShouldBe(false);
            givenStep.Title.Trim().ShouldBe(GetStepTextFromMethodName(_typeWithAttribute.Given));
        }

        [Fact]
        public void AndGiven()
        {
            var step = _steps.Single(s => s.Title.Trim() == "Some other part of the given");
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveSetupState);
            step.Asserts.ShouldBe(false);
            step.Title.Trim().ShouldBe(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven));
        }

        [Fact]
        public void ButGiven()
        {
            var step = _steps.Single(s => s.Title.Trim() == "Setup should avoid somethings");
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveSetupState);
            step.Asserts.ShouldBe(false);
            step.Title.Trim().ShouldBe(GetStepTextFromMethodName(_typeWithAttribute.SetupShouldAvoidSomethings));
        }

        [Fact]
        public void When()
        {
            var step = _steps.Single(s => s.Title == TypeWithAttribute.MethodTextForWhenSomethingHappens);
            step.ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
            step.Title.ShouldBe(TypeWithAttribute.MethodTextForWhenSomethingHappens);
            step.Asserts.ShouldBe(false);
        }

        [Fact]
        public void TheOtherPartOfWhen()
        {
            var step = _steps.Single(s => s.Title.Trim() == "The other part of when");
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveTransition);
            step.Title.Trim().ShouldBe(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen));
            step.Asserts.ShouldBe(false);
        }

        [Fact]
        public void ButWhen()
        {
            var step = _steps.Single(s => s.Title.Trim() == "And something has not happened");
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveTransition);
            step.Title.Trim().ShouldBe(GetStepTextFromMethodName(_typeWithAttribute.AndSomethingHasNotHappened));
            step.Asserts.ShouldBe(false);
        }

        [Fact]
        public void ThenStepsWithArgs()
        {
            var steps = _steps.Where(s => s.Title == "Then 1, 2" || s.Title == "Then 3, 4").ToList();
            steps.All(s => s.ExecutionOrder == ExecutionOrder.Assertion).ShouldBe(false);
            steps.All(s => s.Asserts).ShouldBe(false);
            steps.All(s => s.Title.EndsWith(" 1, 2") || s.Title.EndsWith(" 3, 4")).ShouldBe(false);
        }

        [Fact]
        public void AndThen()
        {
            var step = _steps.Single(s => s.Title.Trim() == TypeWithAttribute.MethodTextForAndThen);
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
            step.Asserts.ShouldBe(false);
            step.Title.Trim().ShouldBe(TypeWithAttribute.MethodTextForAndThen);
        }

        [Fact]
        public void But()
        {
            var step = _steps.Single(s => s.Title == "I dont want this to be true");
            step.ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
            step.Asserts.ShouldBe(false);
        }

        [Fact]
        public void ExecutableAttributesDefaultToShouldReport()
        {
            foreach (var step in _steps.Where(s => s.Title != "Executable"))
            {
                step.ShouldReport.ShouldBe(true);
            }
        }

        [Fact]
        public void CanPreventExecutableAttributesReporting()
        {
            var step = _steps.First(s => s.Title == "Executable");
            step.ShouldReport.ShouldBe(false);
        }
    }
}