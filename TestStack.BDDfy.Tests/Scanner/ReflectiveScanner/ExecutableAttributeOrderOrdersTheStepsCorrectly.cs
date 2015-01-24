using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
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

        public ExecutableAttributeOrderOrdersTheStepsCorrectly()
        {
            var testObject = new TypeWithOrderedAttribute();
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var scanner = new ReflectiveScenarioScanner(stepScanners);
            var scenario = scanner.Scan(TestContext.GetContext(testObject)).First();
            _steps = scenario.Steps;
        }

        [Fact]
        public void Step0IsGiven1()
        {
            _steps[0].Title.ShouldBe("Given 1");
        }

        [Fact]
        public void Step1IsGiven3()
        {
            _steps[1].Title.ShouldBe("Given 3");
        }


        [Fact]
        public void Step2IsAndGivenNeg3()
        {
            _steps[2].Title.ShouldBe("And given neg 3");
        }

        [Fact]
        public void Step3IsAndGiven0()
        {
            _steps[3].Title.ShouldBe("And given 0");
        }


        [Fact]
        public void Step4AndGiven2()
        {
            _steps[4].Title.ShouldBe("And given 2");
        }

        [Fact]
        public void Step5AndGiven2Again()
        {
            _steps[5].Title.ShouldBe("And given 2 again");
        }


        [Fact]
        public void Step6IsWhen1()
        {
            _steps[6].Title.ShouldBe("When 1");
        }

        [Fact]
        public void Step7IsWhen3()
        {
            _steps[7].Title.ShouldBe("When 3");
        }


        [Fact]
        public void Step8IsAndWhenNeg3()
        {
            _steps[8].Title.ShouldBe("And when neg 3");
        }

        [Fact]
        public void Step9IsAndWhen0()
        {
            _steps[9].Title.ShouldBe("And when 0");
        }


        [Fact]
        public void Step10AndWhen2()
        {
            _steps[10].Title.ShouldBe("And when 2");
        }

        [Fact]
        public void Step11AndWhen2Again()
        {
            _steps[11].Title.ShouldBe("And when 2 again");
        }


        [Fact]
        public void Step12IsThen1()
        {
            _steps[12].Title.ShouldBe("Then 1");
        }

        [Fact]
        public void Step13IsThen3()
        {
            _steps[13].Title.ShouldBe("Then 3");
        }


        [Fact]
        public void Step14IsAndThenNeg3()
        {
            _steps[14].Title.ShouldBe("And then neg 3");
        }

        [Fact]
        public void Step15IsAndThen0()
        {
            _steps[15].Title.ShouldBe("And then 0");
        }


        [Fact]
        public void Step16AndThen2()
        {
            _steps[16].Title.ShouldBe("And then 2");
        }

        [Fact]
        public void Step17AndThen2Again()
        {
            _steps[17].Title.ShouldBe("And then 2 again");
        }
    }
}
