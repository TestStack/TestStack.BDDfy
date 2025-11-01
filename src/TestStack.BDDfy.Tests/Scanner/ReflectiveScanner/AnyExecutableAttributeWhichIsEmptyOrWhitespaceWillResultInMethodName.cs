using Shouldly;
using System.Linq;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class AnyExecutableAttributeWhichIsEmptyOrWhitespaceWillResultInMethodName
    {
        private class TypeWithDecoratedMethods
        {
            public TypeWithDecoratedMethods()
            {
                this.WithExamples(new ExampleTable("Example")
                {
                    {1},
                    {2}
                });
            }

            [Given("")]
            public void GivenWithEmptyString() { }
            [When("   ")]
            public void WhenWithWhitespace() { }
            [Then(null)]
            public void ThenWithNull() { }

            [AndThen("Then with title should work too")]
            public void AndThenWithTitle() { }
        }

        [Fact]
        public void WhenUsingReflectiveScanner_MethodNamesAreUsedAsStepTitles_When_StepTitlesAreWhiteSpace()
        {
            var testObject = new TypeWithDecoratedMethods();

            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var scanner = new ReflectiveScenarioScanner(stepScanners);
            var scenario = scanner.Scan(TestContext.GetContext(testObject)).First();
            var steps = scenario.Steps;

            steps[0].Title.ShouldBe("Given with empty string");
            steps[1].Title.ShouldBe("When with whitespace");
            steps[2].Title.ShouldBe("Then with null");
            steps[3].Title.ShouldBe("Then with title should work too");
        }
    }
}
