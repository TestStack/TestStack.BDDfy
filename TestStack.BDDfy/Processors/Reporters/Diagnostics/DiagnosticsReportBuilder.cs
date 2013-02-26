using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Processors.Reporters.Serializers;

namespace TestStack.BDDfy.Processors.Reporters.Diagnostics
{
    public class DiagnosticsReportBuilder : IReportBuilder
    {
        private readonly ISerializer _serializer;

        public DiagnosticsReportBuilder() : this(new JsonSerializer()) { }

        public DiagnosticsReportBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string CreateReport(FileReportModel model)
        {
            var graph = GetDiagnosticData(model);
            var rootObject = new { Stories = graph };
            return _serializer.Serialize(rootObject);
        }

        public IList<StoryDiagnostic> GetDiagnosticData(FileReportModel viewModel)
        {
            var graph = new List<StoryDiagnostic>();
            foreach (var story in viewModel.Stories)
            {
                graph.Add(new StoryDiagnostic()
                {
                    Name = story.MetaData.Title,
                    Duration = story.Scenarios.Sum(x => x.Duration.Milliseconds),
                    Scenarios = story.Scenarios.Select(scenario => new StoryDiagnostic.Scenario()
                    {
                        Name = scenario.Title,
                        Duration = scenario.Duration.Milliseconds,
                        Steps = scenario.Steps.Select(step => new StoryDiagnostic.Step()
                        {
                            Name = step.StepTitle,
                            Duration = step.Duration.Milliseconds
                        }).ToList()
                    }).ToList()
                });
            }

            return graph;
        }
    }
}