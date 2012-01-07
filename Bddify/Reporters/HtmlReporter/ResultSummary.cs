using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Reporters.HtmlReporter
{
    public class ResultSummary
    {
        readonly IEnumerable<Story> _stories;
        readonly IEnumerable<Scenario> _scenarios;

        public ResultSummary(IEnumerable<Story> stories)
        {
            _stories = stories;
            _scenarios = _stories.SelectMany(s => s.Scenarios).ToList();
        }

        public int Namespaces
        {
            get
            {
                return _stories.Where(b => b.MetaData == null)
                    .SelectMany(s => s.Scenarios).GroupBy(b => b.TestObject.GetType().Namespace).Count();
            }
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
    }
}