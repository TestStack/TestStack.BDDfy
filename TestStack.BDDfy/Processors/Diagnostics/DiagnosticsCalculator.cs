using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public class DiagnosticsCalculator : IDiagnosticsCalculator
    {
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