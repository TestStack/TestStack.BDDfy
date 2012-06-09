using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Processors.HtmlReporter;

namespace TestStack.BDDfy.Configuration
{
    public class BatchProcessors
    {
        IEnumerable<IBatchProcessor> _GetProcessors()
        {
            var htmlReporter = HtmlReport.ConstructFor(StoryCache.Stories);
            if (htmlReporter != null)
                yield return htmlReporter;

            var markDown = MarkDownReport.ConstructFor(StoryCache.Stories);
            if (markDown != null)
                yield return markDown;

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor;
            }
        }

        private readonly BatchProcessorFactory _htmlReportFactory = new BatchProcessorFactory(() => new HtmlReporter(new DefaultHtmlReportConfiguration()));
        public BatchProcessorFactory HtmlReport { get { return _htmlReportFactory; } }

        private readonly BatchProcessorFactory _markDownFactory = new BatchProcessorFactory(() => new MarkDownReporter(), false);
        public BatchProcessorFactory MarkDownReport { get { return _markDownFactory; } }

        readonly List<IBatchProcessor> _addedProcessors = new List<IBatchProcessor>();

        public BatchProcessors Add(IBatchProcessor processor)
        {
            _addedProcessors.Add(processor);
            return this;
        }

        public IEnumerable<IBatchProcessor> GetProcessors()
        {
            return _GetProcessors().ToList();
        }
    }
}