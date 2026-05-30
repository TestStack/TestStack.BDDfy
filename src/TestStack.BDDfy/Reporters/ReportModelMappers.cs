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

            foreach (var scenario in story.Scenarios)
            {
                model.Scenarios.Add(scenario.ToScenarioModel());
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
        private static ReportModel.Scenario ToScenarioModel(this Scenario scenario)
        {
            var model = new ReportModel.Scenario
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Tags = scenario.Tags,
                Example = scenario.Example?.ToExampleModel(),
                Duration = scenario.Duration,
                Result = scenario.Result
            };
            scenario.Steps.ForEach(x => model.Steps.Add(x.ToStepModel()));
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

        private static ReportModel.Example ToExampleModel(this Example example)
        {
            return new ReportModel.Example
            {
                Headers = example.Headers,
                Values = example.Values
            };
        }
    }
}