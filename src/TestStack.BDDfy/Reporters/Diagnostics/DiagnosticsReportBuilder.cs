using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Reporters.Serializers;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class DiagnosticsReportBuilder(ISerializer serializer): IReportBuilder
    {
        private readonly ISerializer _serializer = serializer;

        public DiagnosticsReportBuilder() : this(new JsonSerializer()) { }

        public string CreateReport(FileReportModel model)
        {
            var graph = GetDiagnosticData(model);
            var rootObject = new DiagnosticsReport { Stories = graph };
            return _serializer.Serialize(rootObject);
        }

        internal static IList<StoryDiagnostic> GetDiagnosticData(FileReportModel viewModel)
        {
            var graph = new List<StoryDiagnostic>();
            foreach (var story in viewModel.Stories)
            {
                var name = story.Metadata?.Title ?? story.Namespace;

                graph.Add(new StoryDiagnostic
                {
                    Name = name ?? "Story",
                    Duration = story.Scenarios.Sum(x => x.Duration.Milliseconds),
                    Scenarios = [.. story.Scenarios.Select(scenario => new StoryDiagnostic.Scenario()
                    {
                        Name = scenario.Title ?? "Scenario",
                        Duration = scenario.Duration.Milliseconds,
                        Steps = [.. scenario.Steps.Select(step => new StoryDiagnostic.Step()
                        {
                            Name = step.Title ?? "Step",
                            Duration = step.Duration.Milliseconds
                        })]
                    })]
                });
            }

            return graph;
        }
    }
}