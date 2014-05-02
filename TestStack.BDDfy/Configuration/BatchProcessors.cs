using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.MarkDown;

namespace TestStack.BDDfy.Configuration
{
    public class BatchProcessors
    {
        IEnumerable<IBatchProcessor> _GetProcessors()
        {
            var htmlReporter = HtmlReport.ConstructFor(StoryCache.Stories);
            if (htmlReporter != null)
                yield return htmlReporter;

            var htmlMetroReporter = HtmlMetroReport.ConstructFor(StoryCache.Stories);
            if (htmlMetroReporter != null)
                yield return htmlMetroReporter;

            var markDown = MarkDownReport.ConstructFor(StoryCache.Stories);
            if (markDown != null)
                yield return markDown;

            var diagnostics = DiagnosticsReport.ConstructFor(StoryCache.Stories);
            if (diagnostics != null)
                yield return diagnostics;

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor;
            }
        }

        private readonly BatchProcessorFactory _htmlReportFactory = new BatchProcessorFactory(() => new HtmlReporter(new DefaultHtmlReportConfiguration()));
        public BatchProcessorFactory HtmlReport { get { return _htmlReportFactory; } }

        private readonly BatchProcessorFactory _htmlMetroReportFactory = new BatchProcessorFactory(() => new HtmlReporter(new DefaultHtmlReportConfiguration(), new MetroReportBuilder()), false);
        public BatchProcessorFactory HtmlMetroReport { get { return _htmlMetroReportFactory; } }

        private readonly BatchProcessorFactory _markDownFactory = new BatchProcessorFactory(() => new MarkDownReporter(), false);
        public BatchProcessorFactory MarkDownReport { get { return _markDownFactory; } }

        private readonly BatchProcessorFactory _diagnosticsFactory = new BatchProcessorFactory(() => new DiagnosticsReporter(), false);
        public BatchProcessorFactory DiagnosticsReport { get { return _diagnosticsFactory; } }

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