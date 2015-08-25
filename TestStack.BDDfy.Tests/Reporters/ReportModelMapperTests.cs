using System.Linq;
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters
{
    using System.Collections.Generic;

    public class ReportModelMapperTests
    {
        private List<Story> _stories;

        public ReportModelMapperTests()
        {
            _stories = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                .ToList();
        }
            
        [Fact]
        public void story_should_map_to_report_story()
        {
            var mapped = _stories.ToReportModel().Stories;

            mapped.Count.ShouldBe(2);
            for (int i = 0; i < 2; i++)
            {
                mapped[i].Namespace.ShouldBe(_stories[i].Namespace);
                mapped[i].Result.ShouldBe(_stories[i].Result);
                mapped[i].Scenarios.Count.ShouldBe(_stories[i].Scenarios.Count());
                mapped[i].Metadata.ShouldNotBe(null);
            }
        }

        [Fact]
        public void story_metadata_should_map_to_report_story_metadata()
        {
            var mapped = _stories.ToReportModel().Stories;

            for (int i = 0; i < 2; i++)
            {
                mapped[i].Metadata.Narrative1.ShouldBe(_stories[i].Metadata.Narrative1);
                mapped[i].Metadata.Narrative2.ShouldBe(_stories[i].Metadata.Narrative2);
                mapped[i].Metadata.Narrative3.ShouldBe(_stories[i].Metadata.Narrative3);
                mapped[i].Metadata.Title.ShouldBe(_stories[i].Metadata.Title);
                mapped[i].Metadata.TitlePrefix.ShouldBe(_stories[i].Metadata.TitlePrefix);
                mapped[i].Metadata.Type.ShouldBe(_stories[i].Metadata.Type);
                mapped[i].Metadata.ImageUri.ShouldBe(_stories[i].Metadata.ImageUri);
                mapped[i].Metadata.StoryUri.ShouldBe(_stories[i].Metadata.StoryUri);
            }
        }

        [Fact]
        public void scenario_should_map_to_report_scenario()
        {
            var scenarios = _stories[0].Scenarios.ToList();
            var mapped = _stories.ToReportModel().Stories[0].Scenarios;

            for (int i = 0; i < 2; i++)
            {
                mapped[i].Id.ShouldBe(scenarios[i].Id);
                mapped[i].Title.ShouldBe(scenarios[i].Title);
                mapped[i].Example.ShouldNotBe(null);
                mapped[i].Duration.ShouldBe(scenarios[i].Duration);
                mapped[i].Result.ShouldBe(scenarios[i].Result);

                mapped[i].Tags.Count.ShouldBe(scenarios[i].Tags.Count);
                mapped[i].Steps.Count.ShouldBe(scenarios[i].Steps.Count);
            }
        }

        [Fact]
        public void step_should_map_to_report_step()
        {
            var steps = _stories[0].Scenarios.First().Steps;
            var mapped = _stories.ToReportModel().Stories[0].Scenarios.First().Steps;

            for (int i = 0; i < 2; i++)
            {
                mapped[i].Id.ShouldBe(steps[i].Id);
                mapped[i].Asserts.ShouldBe(steps[i].Asserts);
                mapped[i].ShouldReport.ShouldBe(steps[i].ShouldReport);
                mapped[i].Title.ShouldBe(steps[i].Title);
                mapped[i].ExecutionOrder.ShouldBe(steps[i].ExecutionOrder);
                mapped[i].Result.ShouldBe(steps[i].Result);
                mapped[i].Exception.ShouldBe(steps[i].Exception);
                mapped[i].Duration.ShouldBe(steps[i].Duration);
            }
        }

        [Fact]
        public void example_should_map_to_report_example()
        {
            var scenarios = _stories[0].Scenarios.ToList();
            var mapped = _stories.ToReportModel().Stories[0].Scenarios;

            for (int i = 0; i < 2; i++)
            {
                mapped[i].Example.Headers.ShouldBe(scenarios[i].Example.Headers);
                mapped[i].Example.Values.ShouldBe(scenarios[i].Example.Values);
            }
        }
    }
}
