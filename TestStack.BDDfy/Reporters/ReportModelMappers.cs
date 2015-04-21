namespace TestStack.BDDfy.Reporters
{
    using System.Collections.Generic;

    public static class ReportModelMappers
    {
        public static ReportModel ToReportModel(this IEnumerable<Story> stories)
        {
            var report = new ReportModel();
            foreach (var story in stories)
            {
                report.Stories.Add(story.ToStoryModel());
            }

            return report;
        }

        public static ReportModel.Story ToStoryModel(this Story story)
        {
            if (story == null)
                return null;

            var model = new ReportModel.Story
            {
                Namespace = story.Namespace,
                Result = story.Result,
                Metadata = story.Metadata.ToStoryMetadataModel()
            };

            foreach (var scenario in story.Scenarios)
            {
                model.Scenarios.Add(scenario.ToScenarioModel());
            }

            return model;
        }

        public static ReportModel.StoryMetadata ToStoryMetadataModel(this StoryMetadata metadata)
        {
            if (metadata == null)
                return null;

            return new ReportModel.StoryMetadata
            {
                Type = metadata.Type,
                Title = metadata.Title,
                TitlePrefix = metadata.TitlePrefix,
                Narrative1 = metadata.Narrative1,
                Narrative2 = metadata.Narrative2,
                Narrative3 = metadata.Narrative3,
            };
        }
        public static ReportModel.Scenario ToScenarioModel(this Scenario scenario)
        {
            if (scenario == null)
                return null;

            var model = new ReportModel.Scenario
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Tags = scenario.Tags,
                Example = scenario.Example != null ? scenario.Example.ToExampleModel() : null,
                Duration = scenario.Duration,
                Result = scenario.Result
            };
            scenario.Steps.ForEach(x => model.Steps.Add(x.ToStepModel()));
            return model;
        }

        public static ReportModel.Step ToStepModel(this Step step)
        {
            if (step == null)
                return null;

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

        public static ReportModel.Example ToExampleModel(this Example example)
        {
            if (example == null)
                return null;

            return new ReportModel.Example
            {
                Headers = example.Headers,
                Values = example.Values
            };
        }
    }
}