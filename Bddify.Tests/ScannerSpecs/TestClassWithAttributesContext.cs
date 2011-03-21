using System.Collections.Generic;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests.ScannerSpecs
{
    public class TestClassWithAttributesContext
    {
        private TypeWithAttribute _typeWithAttribute;
        private List<ExecutionStep> _steps;

        private class TypeWithAttribute
        {
            [Then]
            public void Then() { }

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
            _steps = new ExecutableAttributeScanner().Scan(_typeWithAttribute).First().Steps.ToList();
        }

        [Test]
        public void DecoratedMethodsAreReturned()
        {
            Assert.That(_steps.Count, Is.EqualTo(6));
        }

        [Test]
        public void GivenIsReturnedFirst()
        {
            Assert.That(_steps[0].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.Given)));
        }

        [Test]
        public void AndGivenIsReturnedSecond()
        {
            Assert.That(_steps[1].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.SomeOtherPartOfTheGiven)));
        }

        [Test]
        public void WhenIsReturnedThird()
        {
            Assert.That(_steps[2].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.When)));
        }

        [Test]
        public void AndWhenIsReturnedForth()
        {
            Assert.That(_steps[3].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.TheOtherPartOfWhen)));
        }

        [Test]
        public void ThenIsReturnedFifth()
        {
            Assert.That(_steps[4].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.Then)));
        }

        [Test]
        public void AndThenIsReturnedSixth()
        {
            Assert.That(_steps[5].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithAttribute.AndThen)));
        }
    }
}