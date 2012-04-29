using System;
using System.Collections.Generic;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
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

        private static readonly ProcessorFactory ConsoleReportFactory = new ProcessorFactory(() => new ConsoleReporter());
        public static ProcessorFactory ConsoleReport { get { return ConsoleReportFactory; } }

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