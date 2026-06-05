namespace TestStack.BDDfy.Reporters
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ReportModelMappers
    {
        public static ReportModel ToReportModel(this IEnumerable<Story> stories)
        {
            var report = new ReportModel();
            foreach (var story in stories.Where(s=> s is not null))
            {
                var storyModel = story.ToStoryModel();
                report.Stories.Add(storyModel);
            }

            return report;
        }

        private static ReportModel.Story ToStoryModel(this Story story)
        {
            var model = new ReportModel.Story
            {
                Namespace = story.Namespace,
                Result = story.Result,
                Metadata = story.Metadata?.ToStoryMetadataModel()
            };

            foreach (var group in story.Scenarios.GroupBy(s => s.Id))
            {
                model.Scenarios.Add(group.ToScenarioModel());
            }

            return model;
        }

        private static ReportModel.StoryMetadata ToStoryMetadataModel(this StoryMetadata metadata)
        {
            return new ReportModel.StoryMetadata
            {
                Type = metadata.Type,
                Title = metadata.Title,
                TitlePrefix = metadata.TitlePrefix,
                Narrative1 = metadata.Narrative1,
                Narrative2 = metadata.Narrative2,
                Narrative3 = metadata.Narrative3,
                ImageUri = metadata.ImageUri,
                StoryUri = metadata.StoryUri
            };
        }
        private static ReportModel.Scenario ToScenarioModel(this IGrouping<string, Scenario> scenarioGroup)
        {
            var first = scenarioGroup.First();
            var model = new ReportModel.Scenario
            {
                Id = first.Id,
                Title = first.Title,
                Tags = first.Tags,
                Duration = new TimeSpan(scenarioGroup.Sum(s => s.Duration.Ticks)),
                Result = (Result)scenarioGroup.Max(s => (int)s.Result)
            };

            first.Steps.ForEach(x => model.Steps.Add(x.ToStepModel()));

            foreach (var scenario in scenarioGroup.Where(s => s.Example is not null))
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                model.Examples.Add(new ReportModel.Example
                {
                    Headers = scenario.Example!.Headers,
                    Values = scenario.Example.Values,
                    Result = scenario.Result,
                    Duration = scenario.Duration,
                    Error = failingStep?.Exception
                });
            }

            return model;
        }

        private static ReportModel.Step ToStepModel(this Step step)
        {
            return new ReportModel.Step
            {
                Id = step.Id,
                Asserts = step.Asserts,
                ShouldReport = step.ShouldReport,
                Title = step.Title,
                ExecutionOrder = step.ExecutionOrder,
                Result = step.Result,
                Exception = step.Exception,
                Duration = step.Duration
            };
        }

            }
        }