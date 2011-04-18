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
            [Then]
            [RunStepWithArgs(1, 2)]
            [RunStepWithArgs(3, 4)]
            public void Then(int input1, int input2) { }

            public void ThenIShouldNotBeReturnedBecauseIDoNotHaveAttributes() { }

            [When]
            public void When() { }

            public void WhenNoAttributeIsProvided() { }

            [Given]
            public void Given() { }

            public void GivenWithoutAttribute() { }

            [AndWhen]
            public void TheOtherPartOfWhen() { }

            [AndThen]
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
        public void WhenIsReturnedThird()
        {
            Assert.That(_steps[2].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.When)));
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
        public void AndWhenStepDoesNotAssert()
        {
            Assert.That(_steps[3].Asserts, Is.False);
        }

        [Test]
        public void ThenIsReturnedFifthAndSixthWithTwoArgSets()
        {
            var thenMethodInfo = _typeWithAttribute.GetType().GetMethod("Then", new [] {typeof(int), typeof(int)});
            Assert.That(_steps[4].Method, Is.EqualTo(thenMethodInfo));
            Assert.That(_steps[4].InputArguments, Is.EqualTo(new object[] { 1, 2 }));
            Assert.That(_steps[5].Method, Is.EqualTo(thenMethodInfo));
            Assert.That(_steps[5].InputArguments, Is.EqualTo(new object[] { 3, 4 }));
        }

        [Test]
        public void ThenStepsDoAssert()
        {
            Assert.That(_steps[4].Asserts, Is.True);
            Assert.That(_steps[5].Asserts, Is.True);
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
    }
}