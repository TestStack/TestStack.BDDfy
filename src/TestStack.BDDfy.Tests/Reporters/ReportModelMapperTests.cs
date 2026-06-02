using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters
{
    using System.Collections.Generic;

    public class ReportModelMapperTests
    {
        private readonly List<Story> _stories;

        public ReportModelMapperTests()
        {
            _stories = [.. new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()];
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
                mapped[i].Metadata.ShouldNotBeNull();
            }
        }

        [Fact]
        public void story_metadata_should_map_to_report_story_metadata()
        {
            var mapped = _stories.ToReportModel().Stories.Select(x => new
            {
                x.Metadata!.Narrative1,
                x.Metadata.Narrative2,
                x.Metadata.Narrative3,
                x.Metadata.Title,
                x.Metadata.TitlePrefix,
                x.Metadata.Type,
                x.Metadata.ImageUri,
                x.Metadata.StoryUri
            }).ToArray();

            var original = _stories.Select(x => new
            {
                x.Metadata!.Narrative1,
                x.Metadata.Narrative2,
                x.Metadata.Narrative3,
                x.Metadata.Title,
                x.Metadata.TitlePrefix,
                x.Metadata.Type,
                x.Metadata.ImageUri,
                x.Metadata.StoryUri
            }).ToArray();

            mapped.ShouldBeEquivalentTo(original);
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
            var scenarios = _stories[0].Scenarios.Select(x=> new {
                x.Example!.Headers,
                x.Example.Values
             }).ToArray();

            var mapped = _stories.ToReportModel().Stories[0].Scenarios.Select(x=> new {
                x.Example!.Headers,
                x.Example.Values
            }).ToArray();

            mapped.ShouldBeEquivalentTo(scenarios);
        }
    }
}
