using System.Collections.Generic;
using Bddify.Core;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.ScannerSpecs
{
    public class WhenTestClassFollowsGivenWhenThenNamingConvention
    {
        private List<ExecutionStep> _steps;
        private TypeWithoutAttribute _typeWithoutAttribute;

        private class TypeWithoutAttribute
        {
            public void AndThen() {}
            public void AndWhen() {}
            public void Given() {}
            public void AndSomething() { }
            public void When() {}
            public void AndGiven() { }
            public void Then(){}
        }

        [SetUp]
        public void Setup()
        {
            _typeWithoutAttribute = new TypeWithoutAttribute();
            _steps = new Scanners.MethodNameScanner().Scan(_typeWithoutAttribute).First().Steps.ToList();
        }
            
        [Test]
        public void AllMethodsFollowingTheNamingConventionAreReturnedAsSteps()
        {
            // AndSomething 
            Assert.That(_steps.Count, Is.EqualTo(6));
        }

        [Test]
        public void GivenIsReturnedFirst()
        {
            Assert.That(_steps[0].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.Given)));
        }

        [Test]
        public void AndGivenIsReturnedSecond()
        {
            Assert.That(_steps[1].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.AndGiven)));
        }

        [Test]
        public void WhenIsReturnedThird()
        {
            Assert.That(_steps[2].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.When)));
        }

        [Test]
        public void AndWhenIsReturnedForth()
        {
            Assert.That(_steps[3].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.AndWhen)));
        }

        [Test]
        public void ThenIsReturnedFifth()
        {
            Assert.That(_steps[4].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.Then)));
        }

        [Test]
        public void AndThenIsReturnedSixth()
        {
            Assert.That(_steps[5].Method, Is.EqualTo(Helpers.GetMethodInfo(_typeWithoutAttribute.AndThen)));
        }
    }
}