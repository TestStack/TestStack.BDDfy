using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
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
