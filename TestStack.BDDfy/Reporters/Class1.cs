using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
    public static class ReportMappers
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
            var model = new ReportModel.Scenario
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Tags = scenario.Tags,
                Example = scenario.Example.ToExampleModel(),
                Duration = scenario.Duration,
                Result = scenario.Result
            };
            scenario.Steps.ForEach(x => model.Steps.Add(x.ToStepModel()));
            return model;
        }

        public static ReportModel.Step ToStepModel(this Step step)
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

        public static ReportModel.Example ToExampleModel(this Example example)
        {
            return new ReportModel.Example
            {
                Headers = example.Headers,
                Values = example.Values
            };
        }
    }

    public class ReportModel
    {
        public List<ReportModel.Story> Stories { get; set; }

        public ReportModel()
        {
            Stories = new List<Story>();
        }

        public class Story
        {
            public Story()
            {
                Scenarios = new List<Scenario>();
            }

            public string Namespace { get; set; }
            public Result Result { get; set; }
            public List<Scenario> Scenarios { get; set; }
            public StoryMetadata Metadata { get; set; }
        }

        public class StoryMetadata
        {
            public Type Type { get; set; }
            public string Title { get; set; }
            public string TitlePrefix { get; set; }
            public string Narrative1 { get; set; }
            public string Narrative2 { get; set; }
            public string Narrative3 { get; set; }
        }

        public class Scenario
        {
            public Scenario()
            {
                Tags = new List<string>();
                Steps = new List<Step>();
            }

            public string Id { get; set; }

            public string Title { get; set; }

            public List<string> Tags { get; set; }

            public Example Example { get; set; }

            public TimeSpan Duration { get; set; }

            public List<Step> Steps { get; set; }

            public Result Result { get; set; }
        }

        public class Step
        {
            public string Id { get; set; }

            public bool Asserts { get; set; }

            public bool ShouldReport { get; set; }

            public string Title { get; set; }

            public ExecutionOrder ExecutionOrder { get; set; }

            public Result Result { get; set; }

            public Exception Exception { get; set; }

            public TimeSpan Duration { get; set; }
        }

        public class Example
        {
            public Example()
            {
                Values = new List<ExampleValue>();
            }

            public string[] Headers { get; set; }

            public IEnumerable<ExampleValue> Values { get; set; }

            public override string ToString()
            {
                return string.Join(", ", Values.Select(i => i.ToString()));
            }
        }
    }
}
