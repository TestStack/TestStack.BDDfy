using System;
using System.Collections.Generic;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify.Core
{
    public static class Configurator
    {
        public static Func<IEnumerable<IProcessor>> Processors = () => 
            new List<IProcessor>
            {
                new TestRunner(),
                new ConsoleReporter(),
                new HtmlReportProcessor(),
                new ExceptionProcessor()
            };

        public static Func<IEnumerable<IStepScanner>> StepScanners = () => 
            new IStepScanner[]
                {
                    new ExecutableAttributeStepScanner(), 
                    new DefaultMethodNameStepScanner()
                };

        public static Func<IStoryMetaDataScanner> StoryMetaDataScanner = () => new StoryAttributeMetaDataScanner();

        public static Func<IEnumerable<IHtmlReportConfiguration>> HtmlReportConfigurations = () => 
            new IHtmlReportConfiguration[]
                {
                    new DefaultHtmlReportConfiguration()
                };
    }
}