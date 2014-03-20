using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
    public class ExecutableAttributeOrderOrdersTheStepsCorrectly
    {
        private List<Step> _steps;

        private class TypeWithOrderedAttribute
        {
            [AndThen(Order = 2)]
            public void AndThen2() { }

            [AndThen(Order = -3)]
            public void AndThenNeg3() { }

            [AndThen]
            public void AndThen0() { }

            [AndThen(Order = 2)]
            public void AndThen2Again() { }

            [Then(Order = 1)]
            public void Then1() { }

            [Then(Order = 3)]
            public void Then3() { }

            [Given(Order = 1)]
            public void Given1() { }

            [Given(Order = 3)]
            public void Given3() { }

            [AndGiven]
            public void AndGiven0() { }

            [AndGiven(Order = 2)]
            public void AndGiven2() { }

            [AndGiven(Order = -3)]
            public void AndGivenNeg3() { }

            [AndGiven(Order = 2)]
            public void AndGiven2Again() { }

            [When(Order = 1)]
            public void When1() { }

            [When(Order = 3)]
            public void When3() { }

            [AndWhen(Order = 2)]
            public void AndWhen2() { }

            [AndWhen(Order = -3)]
            public void AndWhenNeg3() { }

            [AndWhen]
            public void AndWhen0() { }

            [AndWhen(Order = 2)]
            public void AndWhen2Again() { }

        }

        [SetUp]
        public void WhenStep_TestClassHasAttributes()
        {
            var testObject = new TypeWithOrderedAttribute();
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var scanner = new ReflectiveScenarioScanner(stepScanners);
            var scenario = scanner.Scan(testObject);
            _steps = scenario.Steps;
        }

        [Test]
        public void Step0IsGiven1()
        {
            Assert.AreEqual("Given 1", _steps[0].Title);
        }

        [Test]
        public void Step1IsGiven3()
        {
            Assert.AreEqual("Given 3", _steps[1].Title);
        }


        [Test]
        public void Step2IsAndGivenNeg3()
        {
            Assert.AreEqual("And given neg 3", _steps[2].Title);
        }

        [Test]
        public void Step3IsAndGiven0()
        {
            Assert.AreEqual("And given 0", _steps[3].Title);
        }


        [Test]
        public void Step4AndGiven2()
        {
            Assert.AreEqual("And given 2", _steps[4].Title);
        }

        [Test]
        public void Step5AndGiven2Again()
        {
            Assert.AreEqual("And given 2 again", _steps[5].Title);
        }

        
        [Test]
        public void Step6IsWhen1()
        {
            Assert.AreEqual("When 1", _steps[6].Title);
        }

        [Test]
        public void Step7IsWhen3()
        {
            Assert.AreEqual("When 3", _steps[7].Title);
        }


        [Test]
        public void Step8IsAndWhenNeg3()
        {
            Assert.AreEqual("And when neg 3", _steps[8].Title);
        }

        [Test]
        public void Step9IsAndWhen0()
        {
            Assert.AreEqual("And when 0", _steps[9].Title);
        }


        [Test]
        public void Step10AndWhen2()
        {
            Assert.AreEqual("And when 2", _steps[10].Title);
        }

        [Test]
        public void Step11AndWhen2Again()
        {
            Assert.AreEqual("And when 2 again", _steps[11].Title);
        }


        [Test]
        public void Step12IsThen1()
        {
            Assert.AreEqual("Then 1", _steps[12].Title);
        }

        [Test]
        public void Step13IsThen3()
        {
            Assert.AreEqual("Then 3", _steps[13].Title);
        }


        [Test]
        public void Step14IsAndThenNeg3()
        {
            Assert.AreEqual("And then neg 3", _steps[14].Title);
        }

        [Test]
        public void Step15IsAndThen0()
        {
            Assert.AreEqual("And then 0", _steps[15].Title);
        }


        [Test]
        public void Step16AndThen2()
        {
            Assert.AreEqual("And then 2", _steps[16].Title);
        }

        [Test]
        public void Step17AndThen2Again()
        {
            Assert.AreEqual("And then 2 again", _steps[17].Title);
        }
    }
}
