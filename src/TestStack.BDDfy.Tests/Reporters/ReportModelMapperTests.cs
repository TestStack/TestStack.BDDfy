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
                mapped[i].Scenarios.Count.ShouldBe(1); // grouped by scenario id
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

            mapped.Count.ShouldBe(1);
            var mappedScenario = mapped[0];
            mappedScenario.Id.ShouldBe(scenarios[0].Id);
            mappedScenario.Title.ShouldBe(scenarios[0].Title);
            mappedScenario.Examples.Count.ShouldBe(2);
            mappedScenario.Duration.ShouldBe(new TimeSpan(scenarios.Sum(s => s.Duration.Ticks)));
            mappedScenario.Result.ShouldBe((Result)scenarios.Max(s => (int)s.Result));

            mappedScenario.Tags.Count.ShouldBe(scenarios[0].Tags.Count);
            mappedScenario.Steps.Count.ShouldBe(scenarios[0].Steps.Count);
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

            var mapped = _stories.ToReportModel().Stories[0].Scenarios[0].Examples;

            mapped.Count.ShouldBe(2);
            for (int i = 0; i < 2; i++)
            {
                mapped[i].Headers.ShouldBe(scenarios[i].Example!.Headers);
                mapped[i].Values.ShouldBeEquivalentTo(scenarios[i].Example.Values);
            }
        }
    }
}
