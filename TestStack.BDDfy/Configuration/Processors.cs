using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Configuration
{
    public class Processors
    {
        IEnumerable<IProcessor> _GetProcessors(Story story)
        {
            var runner = TestRunner.ConstructFor(story);
            if (runner != null)
                yield return runner;

            var consoleReporter = ConsoleReport.ConstructFor(story);
            if (consoleReporter != null)
                yield return consoleReporter;

            yield return new ExceptionProcessor();

            var storyCache = StoryCache.ConstructFor(story);
            if (storyCache != null)
                yield return storyCache;

            yield return new Disposer();

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor();
            }
        }

        private readonly TestRunnerFactory _testRunnerFactory = new TestRunnerFactory(() => new TestRunner());
        public TestRunnerFactory TestRunner { get { return _testRunnerFactory; } }

        private readonly ProcessorFactory _consoleReportFactory = new ProcessorFactory(() => new ConsoleReporter());
        public ProcessorFactory ConsoleReport { get { return _consoleReportFactory; } }

        private readonly ProcessorFactory _storyCacheFactory = new ProcessorFactory(() => new StoryCache());
        public ProcessorFactory StoryCache { get { return _storyCacheFactory; } }

        readonly List<Func<IProcessor>> _addedProcessors = new List<Func<IProcessor>>();

        public Processors Add(Func<IProcessor> processorFactory)
        {
            _addedProcessors.Add(processorFactory);
            return this;
        }

        public IEnumerable<IProcessor> GetProcessors(Story story)
        {
            var pipeline = from processor in _GetProcessors(story)
                           orderby processor.ProcessType
                           select processor;

            return pipeline.ToList();
        }
    }
}