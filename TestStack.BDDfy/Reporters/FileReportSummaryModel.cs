using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
    public class FileReportSummaryModel
    {
        readonly IEnumerable<ReportModel.Story> _stories;
        readonly IEnumerable<ReportModel.Scenario> _scenarios;

        public FileReportSummaryModel(ReportModel reportModel)
        {
            _stories = reportModel.Stories;
            _scenarios = _stories.SelectMany(s => s.Scenarios).ToList();
        }

        public int Namespaces
        {
            get
            {
                return _stories.Where(b => b.Metadata == null).GroupBy(s => s.Namespace).Count();
            }
        }

        public int Scenarios
        {
            get { return _stories.SelectMany(s => s.Scenarios).Count(); }
        }

        public int Stories
        {
            get { return _stories.Where(b => b.Metadata != null).GroupBy(b => b.Metadata.Type).Count(); }
        }

        public int Passed
        {
            get { return _scenarios.Count(b => b.Result == Result.Passed); }
        }

        public int Failed
        {
            get { return _scenarios.Count(b => b.Result == Result.Failed); }
        }

        public int Inconclusive
        {
            get { return _scenarios.Count(b => b.Result == Result.Inconclusive); }
        }

        public int NotImplemented
        {
            get { return _scenarios.Count(b => b.Result == Result.NotImplemented); }
        }
    }
}