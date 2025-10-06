using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters;

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

        public TestRunnerFactory TestRunner { get; } = new(() => new TestRunner());

        public ProcessorFactory ConsoleReport { get; } = new(() => new ConsoleReporter());

        public ProcessorFactory StoryCache { get; } = new(() => new StoryCache());

        readonly List<Func<IProcessor>> _addedProcessors = [];

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