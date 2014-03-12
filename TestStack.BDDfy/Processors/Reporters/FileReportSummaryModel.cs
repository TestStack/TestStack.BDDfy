using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.Reporters
{
    public class FileReportSummaryModel
    {
        readonly IEnumerable<Story> _stories;
        readonly IEnumerable<Scenario> _scenarios;

        public FileReportSummaryModel(IEnumerable<Story> stories)
        {
            _stories = stories;
            _scenarios = _stories.SelectMany(s => s.Scenarios).ToList();
        }

        public int Namespaces
        {
            get
            {
                return _stories.Where(b => b.MetaData == null).GroupBy(s => s.Namespace).Count();
            }
        }

        public int Scenarios
        {
            get { return _stories.SelectMany(s => s.Scenarios).Count(); }
        }

        public int Stories
        {
            get { return _stories.Where(b => b.MetaData != null).GroupBy(b => b.MetaData.Type).Count(); }
        }

        public int Passed
        {
            get { return _scenarios.Count(b => b.Result == StepExecutionResult.Passed); }
        }

        public int Failed
        {
            get { return _scenarios.Count(b => b.Result == StepExecutionResult.Failed); }
        }

        public int Inconclusive
        {
            get { return _scenarios.Count(b => b.Result == StepExecutionResult.Inconclusive); }
        }

        public int NotImplemented
        {
            get { return _scenarios.Count(b => b.Result == StepExecutionResult.NotImplemented); }
        }
    }
}