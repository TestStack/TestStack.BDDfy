using System;
using System.Collections.Generic;
using Bddify.Reporters.HtmlReporter;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify.Configuration
{
    public class ScannerConfig
    {
        public static Func<IEnumerable<IStepScanner>> StepScanners = () =>
                                                                     new IStepScanner[]
                                                                         {
                                                                             new ExecutableAttributeStepScanner(), 
                                                                             new DefaultMethodNameStepScanner()
                                                                         };

        public Func<IStoryMetaDataScanner> StoryMetaDataScanner = () => new StoryAttributeMetaDataScanner();

        public IEnumerable<IHtmlReportConfiguration> HtmlReportConfigurations =
            new IHtmlReportConfiguration[]
                {
                    new DefaultHtmlReportConfiguration()
                };
    }
}