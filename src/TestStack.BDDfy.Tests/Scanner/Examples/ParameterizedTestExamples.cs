using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    public class ParameterizedTestExamples
    {
        private string _input = null!;
        private int _expected;
        private string _result = null!;

        private void GivenInput()
        {
            // input is already set via field by the test framework
        }

        private void WhenProcessed()
        {
            _result = _input?.ToUpperInvariant() ?? string.Empty;
        }

        private void ThenTheResultShouldBeExpected()
        {
            _result.Length.ShouldBe(_expected);
        }

        [Theory]
        [InlineData("hello", 5)]
        [InlineData("world", 5)]
        [InlineData("hi", 2)]
        public void ShouldGroupParameterizedTestsAsExamples(string input, int expected)
        {
            _input = input;
            _expected = expected;
            this.BDDfy();
        }

        [Theory]
        [InlineData("abc", 3)]
        [InlineData("de", 2)]
        public void FluentShouldGroupParameterizedTestsAsExamples(string input, int expected)
        {
            _input = input;
            _expected = expected;
            this.Given(_ => GivenInput())
                .When(_ => WhenProcessed())
                .Then(_ => ThenTheResultShouldBeExpected())
                .BDDfy();
        }

        [Fact]
        public void ParameterizedTestsShouldShareScenarioId()
        {
            // Verify that grouped scenarios have Examples via existing test data
            var stories = new ReportTestData()
                .CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                .ToReportModel();

            foreach (var story in stories.Stories)
            {
                foreach (var scenario in story.Scenarios)
                {
                    if (scenario.Examples.Count > 0)
                    {
                        scenario.Examples.Count.ShouldBeGreaterThan(1);
                    }
                }
            }
        }

        [Theory]
        [InlineData("hello", 5)]
        [InlineData("world", 5)]
        public void ScenarioShouldHaveExampleWhenParameterized(string input, int expected)
        {
            _input = input;
            _expected = expected;

            var story = this.BDDfy();
            var scenario = story.Scenarios.First();

            // The scenario should have an Example attached from the parameterized test detection
            scenario.Example.ShouldNotBeNull();
            scenario.Example.Headers.ShouldContain("input");
            scenario.Example.Headers.ShouldContain("expected");
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData("bb", 2)]
        public void AllInvocationsShouldShareSameScenarioId(string input, int expected)
        {
            _input = input;
            _expected = expected;

            var story = this.BDDfy();
            var scenario = story.Scenarios.First();

            // The scenario ID should be stable (based on type + method name)
            // All invocations of this test method should produce the same ID
            var expectedId = $"scenario-{GetType().FullName}.AllInvocationsShouldShareSameScenarioId".GetHashCode().ToString("x8");
            scenario.Id.ShouldBe(expectedId);
        }

        [Fact]
        public void FileReportModelShouldMergeScenariosWithSameIdAcrossStories()
        {
            // Simulate what happens when multiple parameterized test invocations
            // produce separate stories with scenarios sharing the same stable ID
            var sharedId = "shared-scenario-id";
            var storyMetadata = new StoryMetadata(typeof(ParameterizedTestExamples), "As a tester", "I want grouped examples", "So reports are clean", "Parameterized tests");

            var example1 = new Example(new ExampleValue("input", "hello", () => 0), new ExampleValue("expected", 5, () => 0));
            var example2 = new Example(new ExampleValue("input", "world", () => 1), new ExampleValue("expected", 5, () => 1));

            var scenario1 = new Scenario(sharedId, this, [], "Should group parameterized tests", example1, []);
            var scenario2 = new Scenario(sharedId, this, [], "Should group parameterized tests", example2, []);

            var story1 = new Story(storyMetadata, scenario1);
            var story2 = new Story(storyMetadata, scenario2);

            var reportModel = new[] { story1, story2 }.ToReportModel();
            var fileModel = new FileReportModel(reportModel);

            // After aggregation, there should be one story with one scenario that has 2 examples
            var aggregatedStory = fileModel.Stories.Single();
            aggregatedStory.Scenarios.Count.ShouldBe(1);
            aggregatedStory.Scenarios[0].Examples.Count.ShouldBe(2);
            aggregatedStory.Scenarios[0].Id.ShouldBe(sharedId);
        }
    }
}
