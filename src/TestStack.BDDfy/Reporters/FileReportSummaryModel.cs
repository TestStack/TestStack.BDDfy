using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
    public class FileReportSummaryModel(ReportModel reportModel)
    {
        readonly IEnumerable<ReportModel.Story> _stories = reportModel.Stories;
        readonly IEnumerable<ReportModel.Scenario> _scenarios = [.. reportModel.Stories.SelectMany(s => s.Scenarios)];

        public int Namespaces => _stories.Where(b => b.Metadata == null).GroupBy(s => s.Namespace).Count();

        public int Scenarios => _stories.SelectMany(s => s.Scenarios).Count();

        public int Stories => _stories.Where(b => b.Metadata is not null).GroupBy(b => b.Metadata!.Type).Count();

        public int Passed => _scenarios.Count(b => b.Result is Result.Passed);

        public int Failed => _scenarios.Count(b => b.Result is Result.Failed);

        public int Inconclusive => _scenarios.Count(b => b.Result is Result.Inconclusive);

        public int NotImplemented => _scenarios.Count(b => b.Result is Result.NotImplemented);
    }
}