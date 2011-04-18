using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Scanner
{
    public class WhenTestClassFollowsGivenWhenThenNamingConvention
    {
        private List<ExecutionStep> _steps;
        private TypeWithoutAttribute _typeWithoutAttribute;

        private class TypeWithoutAttribute
        {
            public void AndThen() {}
            public void AndWhen1() {}
            public void AndWhen2() {}
            public void Given() {}
            public void AndSomething() { }
            public void When() {}
            public void AndGiven1() { }
            public void AndGiven2() { }
            public void Then(){}
        }

        [SetUp]
        public void Setup()
        {
            _typeWithoutAttribute = new TypeWithoutAttribute();
            _steps = new DefaultScanForStepsByMethodName().Scan(typeof(TypeWithoutAttribute)).ToList();
        }
            
        [Test]
        public void AllMethodsFollowingTheNamingConventionAreReturnedAsSteps()
        {
            // AndSomething 
            Assert.That(_steps.Count, Is.EqualTo(9));
        }

        private static void AssertStep(ExecutionStep step, MethodInfo methodInfo, bool asserts = false)
        {
            Assert.That(step.Method, Is.EqualTo(methodInfo));
            Assert.That(step.Asserts, Is.EqualTo(asserts));
            Assert.That(step.ShouldReport, Is.True);
        }

        [Test]
        public void GivenIsReturnedFirst()
        {
            AssertStep(_steps[0], Helpers.GetMethodInfo(_typeWithoutAttribute.Given));
        }

        [Test]
        public void AndGiven1IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[1], Helpers.GetMethodInfo(_typeWithoutAttribute.AndGiven1));
        }

        [Test]
        public void AndGiven2IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[2], Helpers.GetMethodInfo(_typeWithoutAttribute.AndGiven2));
        }

        [Test]
        public void WhenIsReturnedAfterGivens()
        {
            AssertStep(_steps[3], Helpers.GetMethodInfo(_typeWithoutAttribute.When));
        }

        [Test]
        public void AndWhen1IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[4], Helpers.GetMethodInfo(_typeWithoutAttribute.AndWhen1));
        }

        [Test]
        public void AndWhen2IsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[5], Helpers.GetMethodInfo(_typeWithoutAttribute.AndWhen2));
        }

        [Test]
        public void ThenIsReturnedAfterWhens()
        {
            AssertStep(_steps[6], Helpers.GetMethodInfo(_typeWithoutAttribute.Then), true);
        }

        [Test]
        public void AndThenIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[7], Helpers.GetMethodInfo(_typeWithoutAttribute.AndThen), true);
        }

        [Test]
        public void AndSomethingIsReturnedInTheCorrectSpot()
        {
            AssertStep(_steps[8], Helpers.GetMethodInfo(_typeWithoutAttribute.AndSomething), true);
        }
    }
}