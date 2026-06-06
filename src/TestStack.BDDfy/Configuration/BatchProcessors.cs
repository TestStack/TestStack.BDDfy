using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.MarkDown;

namespace TestStack.BDDfy.Configuration
{
    public class BatchProcessors
    {
        readonly List<IBatchProcessor> _addedProcessors = [];
        readonly List<Action<IBatchProcessor>> _batchProcessorConfigurators = [];

        private IEnumerable<IBatchProcessor> YieldProcessors()
        {
            var htmlReporter = HtmlReport.ConstructFor(StoryCache.Stories);
            if (htmlReporter != null)
                yield return Configured(htmlReporter);

            var markDown = MarkDownReport.ConstructFor(StoryCache.Stories);
            if (markDown != null)
                yield return Configured(markDown);

            var diagnostics = DiagnosticsReport.ConstructFor(StoryCache.Stories);
            if (diagnostics != null)
                yield return Configured(diagnostics);

            var jsonReporter = JsonDataFileReport.ConstructFor(StoryCache.Stories);
            if (jsonReporter != null)
                yield return Configured(jsonReporter);

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor;
            }
        }

        public BatchProcessorFactory HtmlReport { get; } = new(() => new HtmlReporter(), false);

        public BatchProcessorFactory MarkDownReport { get; } = new(() => new GenericReporter<MarkDownReportBuilder>("bddfy-report.md"), false);

        public BatchProcessorFactory DiagnosticsReport { get; } = new(() => new GenericReporter<DiagnosticsReportBuilder>("bddfy-diagnostics.json"), false);
        
        public BatchProcessorFactory JsonDataFileReport { get; } = new(() => new JsonDataFileReporter(), false);

        public BatchProcessors Add(IBatchProcessor processor)
        {
            _addedProcessors.Add(processor);
            return this;
        }

        public IEnumerable<IBatchProcessor> GetProcessors()
        {
            return [.. YieldProcessors()];
        }

        public void Configure<TProcessor>(Action<TProcessor> configure) where TProcessor : IBatchProcessor
        {
            _batchProcessorConfigurators.Add(processor =>
            {
                if (processor is TProcessor typedProcessor)
                    configure(typedProcessor);
            });
        }

        private TProcessor Configured<TProcessor>(TProcessor processor) where TProcessor : IBatchProcessor
        {
            foreach (var configurator in _batchProcessorConfigurators)
            {
                configurator(processor);
            }

            return processor;
        }
    }
}