using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters.HtmlReporter;

namespace Bddify.Configuration
{
    public class Pipeline
    {
        IEnumerable<IProcessor> _GetProcessors(Story story)
        {
            yield return new TestRunner();
            yield return new HtmlReporter();

            var consoleReporter = Factory.ConsoleReport.ConstructFor(story);
            if (consoleReporter != null)
                yield return consoleReporter;

            yield return new ExceptionProcessor();

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor();
            }
        }

        readonly List<Func<IProcessor>> _addedProcessors = new List<Func<IProcessor>>();

        public Pipeline Add(Func<IProcessor> processorFactory)
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