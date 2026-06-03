using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class ExecutableAttributeShould
    {
        private class TypeWithOrderedAttribute
        {
            [AndThen(Order = 2)]
            public void AndThenOutcome05() { }

            [AndThen(Order = -3)]
            public void AndThenOutcome03() { }

            [AndThen]
            public void AndThenOutcome04() { }

            [AndThen(Order = 2)]
            public void AndThenOutcome06() { }

            [Then(Order = 1)]
            public void ThenOutcome01() { }

            [Then(Order = 3)]
            public void ThenOutcome02() { }

            [Given(Order = 1)]
            public void GivenState01() { }

            [Given(Order = 3)]
            public void GivenState02() { }

            [AndGiven]
            public void AndGivenState04() { }

            [AndGiven(Order = 2)]
            public void AndGivenState05() { }

            [AndGiven(Order = -3)]
            public void AndGivenState03() { }

            [AndGiven(Order = 2)]
            public void AndGivenState06() { }

            [When(Order = 1)]
            public void WhenAction01() { }

            [When(Order = 3)]
            public void WhenAction02() { }

            [AndWhen(Order = 2)]
            public void AndWhenAction05() { }

            [AndWhen(Order = -3)]
            public void AndWhenAction03() { }

            [AndWhen]
            public void AndWhenAction04() { }

            [AndWhen(Order = 2)]
            public void AndWhenAction06() { }

        }

        [Fact]
        public void OrderTheStepsCorrectly()
        {
            var testObject = new TypeWithOrderedAttribute();
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var scanner = new ReflectiveScenarioScanner(stepScanners);
            var scenario = scanner.Scan(TestContext.GetContext(testObject)).First();

            var actualTitles = scenario.Steps.Select(s => s.Title).ToArray();
            var expectedTitles = new[]
            {
                "Given state 01",
                "And state 02",
                "And state 03",
                "And state 04",
                "And state 05",
                "And state 06",
                "When action 01",
                "And action 02",
                "And action 03",
                "And action 04",
                "And action 05",
                "And action 06",
                "Then outcome 01",
                "And outcome 02",
                "And outcome 03",
                "And outcome 04",
                "And outcome 05",
                "And outcome 06"
            };

            actualTitles.ShouldBe(expectedTitles);
        }
    }
}
