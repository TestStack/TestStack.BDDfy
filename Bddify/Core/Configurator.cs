using System;
using System.Collections.Generic;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;
using System.Linq;

namespace Bddify.Core
{
    public static class Configurator
    {
        private static readonly Pipeline PipelineFactory = new Pipeline();

        public static Pipeline Pipeline 
        {
            get
            {
                return PipelineFactory;
            }
        }

        public static Func<IEnumerable<IStepScanner>> StepScanners = () => 
            new IStepScanner[]
                {
                    new ExecutableAttributeStepScanner(), 
                    new DefaultMethodNameStepScanner()
                };

        public static Func<IStoryMetaDataScanner> StoryMetaDataScanner = () => new StoryAttributeMetaDataScanner();

        public static IEnumerable<IHtmlReportConfiguration> HtmlReportConfigurations = 
            new IHtmlReportConfiguration[]
                {
                    new DefaultHtmlReportConfiguration()
                };
    }

    public class Pipeline
    {
        Predicate<Story> _consoleReportRunsOn = story => true;

        IEnumerable<Func<IProcessor>> DefaultPipeline
        {
            get
            {
                return new List<Func<IProcessor>>
                    {
                        () => new TestRunner(),
                        () => new ConsoleReporter(_consoleReportRunsOn),
                        () => new HtmlReportProcessor(),
                        () => new ExceptionProcessor()
                    }.AsReadOnly();
            }
        }

        readonly List<Func<IProcessor>> _addedProcessors = new List<Func<IProcessor>>();

        public Pipeline Add(Func<IProcessor> processorFactory)
        {
            _addedProcessors.Add(processorFactory);
            return this;
        }

        public Pipeline RunConsoleReportOnlyWhen(Predicate<Story> runsOn)
        {
            _consoleReportRunsOn = runsOn;
            return this;
        }

        public IEnumerable<IProcessor> Processors
        {
            get
            {
                var pipeline = from processorFactory in DefaultPipeline.Union(_addedProcessors)
                               let processor = processorFactory()
                               orderby processor.ProcessType
                               select processor;

                return pipeline.ToList();
            }
        }
    }
}