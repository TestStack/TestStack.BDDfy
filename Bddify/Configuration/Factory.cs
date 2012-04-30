using System;
using System.Collections.Generic;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
using Bddify.Reporters.MarkDownReporter;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify.Configuration
{
    public static class Factory
    {
        private static readonly Pipeline PipelineFactory = new Pipeline();

        public static Pipeline Pipeline 
        {
            get
            {
                return PipelineFactory;
            }
        }

        private static readonly ProcessorFactory TestRunnerFactory = new ProcessorFactory(() => new TestRunner());
        public static ProcessorFactory TestRunner { get { return TestRunnerFactory; } }

        private static readonly ProcessorFactory ConsoleReportFactory = new ProcessorFactory(() => new ConsoleReporter());
        public static ProcessorFactory ConsoleReport { get { return ConsoleReportFactory; } }

        private static readonly ProcessorFactory HtmlReportFactory = new ProcessorFactory(() => new HtmlReporter());
        public static ProcessorFactory HtmlReport { get { return HtmlReportFactory; } }

        private static readonly ProcessorFactory MarkdownFactory = new ProcessorFactory(() => new MarkDownReporter(), false);
        public static ProcessorFactory MarkdownReport { get { return MarkdownFactory; } }

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
}