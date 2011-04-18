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
            return NetToString.FromName(Helpers.GetMethodInfo(methodInfoAction).Name);
        }

        [Test]
        public void DecoratedMethodsAreReturned()
        {
            Assert.That(_steps.Count, Is.EqualTo(7));
        }

        [Test]
        public void GivenIsReturnedFirst()
        {
            Assert.That(_steps[0].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.Given)));
        }

        [Test]
        public void GivenStepDoesNotAssert()
        {
            Assert.That(_steps[0].Asserts, Is.False);
        }

        [Test]
        public void GivenStepTextIsFetchedFromMethodName()
        {
            Assert.That(_steps[0].ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.Given)));
        }

        [Test]
        public void AndGivenIsReturnedSecond()
        {
            Assert.That(_steps[1].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        [Test]
        public void AndGivenStepDoesNotAssert()
        {
            Assert.That(_steps[1].Asserts, Is.False);
        }

        [Test]
        public void AndGivenStepTextIsFetchedFromMethodName()
        {
            Assert.That(_steps[1].ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        [Test]
        public void WhenIsReturnedThird()
        {
            Assert.That(_steps[2].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.When)));
        }

        [Test]
        public void WhenStepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(_steps[2].ReadableMethodName, Is.EqualTo(TypeWithAttribute.MethodTextForWhenSomethingHappens));
        }

        [Test]
        public void WhenStepDoesNotAssert()
        {
            Assert.That(_steps[2].Asserts, Is.False);
        }

        [Test]
        public void AndWhenIsReturnedForth()
        {
            Assert.That(_steps[3].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.TheOtherPartOfWhen)));
        }

        [Test]
        public void AndWhenStepTextIsFetchedFromMethodName()
        {
            Assert.That(_steps[3].ReadableMethodName, Is.EqualTo(GetStepTextFromMethodName(_typeWithAttribute.TheOtherPartOfWhen)));
        }

        [Test]
        public void AndWhenStepDoesNotAssert()
        {
            Assert.That(_steps[3].Asserts, Is.False);
        }

        [TestCase(4)]
        [TestCase(5)]
        public void ThenStepIsReturnedInTheCorrectSpot(int stepIndex)
        {
            MethodInfo thenMethodInfo = GetThenMethodInfo();
            Assert.That(_steps[stepIndex].Method, Is.EqualTo(thenMethodInfo));
        }

        [TestCase(4)]
        [TestCase(5)]
        public void ThenStepIsProvidedWithCorrectArguments(int stepIndex)
        {
            Assert.That(_steps[stepIndex].InputArguments, Is.Not.Null);
            Assert.That(_steps[stepIndex].InputArguments.Length, Is.EqualTo(2));

            // steps are not in the order attributes are provided.
            // so make sure that the first argument in the arg set matches one of the first arguments
            // and the same for the second arguments
            Assert.That(new object[] {1, 3}, Contains.Item(_steps[stepIndex].InputArguments[0]));
            Assert.That(new object[] {2, 4}, Contains.Item(_steps[stepIndex].InputArguments[1]));
        }

        private MethodInfo GetThenMethodInfo()
        {
            return _typeWithAttribute.GetType().GetMethod("Then", new [] {typeof(int), typeof(int)});
        }

        [Test]
        public void ThenStepsDoAssert()
        {
            Assert.That(_steps[4].Asserts, Is.True);
            Assert.That(_steps[5].Asserts, Is.True);
        }

        [TestCase(4)]
        [TestCase(5)]
        public void ThenStepTextIsFetchedFromMethodNamePostfixedWithArguments(int thenStepIndex)
        {
            Assert.IsTrue(_steps[thenStepIndex].ReadableMethodName.Contains(" with args ("));
            Assert.IsTrue(_steps[thenStepIndex].ReadableMethodName.Contains(NetToString.FromName(GetThenMethodInfo().Name)));
        }

        [Test]
        public void AndThenIsReturnedSixth()
        {
            Assert.That(_steps[6].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.AndThen)));
        }

        [Test]
        public void AndThenStepAsserts()
        {
            Assert.That(_steps[6].Asserts, Is.True);
        }

        [Test]
        public void AndThenStepTextIsFetchedFromExecutableAttribute()
        {
            Assert.That(_steps[6].ReadableMethodName, Is.EqualTo(TypeWithAttribute.MethodTextForAndThen));
        }
    }
}