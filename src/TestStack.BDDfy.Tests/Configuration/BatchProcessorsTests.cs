using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.MarkDown;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Configuration
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class BatchProcessorsTests
    {
        static bool MetroReportProcessorIsActive(IBatchProcessor batchProcessor)
        {
            return batchProcessor is HtmlReporter reporter && reporter.ReportBuilder is MetroReportBuilder;
        }

        [Fact]
        public void ReturnsHtmlReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(p => p is HtmlReporter).ShouldBe(true);
        }

        [Fact]
        public void DoesNotReturnMarkDownReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(p => p is MarkDownReporter).ShouldBe(false);
        }

        [Fact]
        public void DoesNotReturnHtmlMetroReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(MetroReportProcessorIsActive).ShouldBe(false);
        }

        [Fact]
        public void DoesNotReturnHtmlReporterWhenItIsDeactivated()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(p => p is HtmlReporter).ShouldBe(false);

            Configurator.BatchProcessors.HtmlReport.Enable();
        }

        [Fact]
        public void ReturnsMarkdownReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.MarkDownReport.Enable();
            
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(p => p is MarkDownReporter).ShouldBe(true);

            Configurator.BatchProcessors.MarkDownReport.Disable();
        }

        [Fact]
        public void ReturnsHtmlMetroReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.HtmlMetroReport.Enable();
            
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(MetroReportProcessorIsActive).ShouldBe(true);

            Configurator.BatchProcessors.HtmlMetroReport.Disable();
        }

        [Fact]
        public void ReturnsDianosticsReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.DiagnosticsReport.Enable();

            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            processors.ShouldContain(p=> p is DiagnosticsReporter, 1);

            Configurator.BatchProcessors.DiagnosticsReport.Disable();
        }

        [Fact]
        public void ReturnsAdditionalBatchProcessorsWhenAdded()
        {
            Configurator.BatchProcessors.Add(new FooBatchProcessor());

            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            processors.ShouldContain(p => p is FooBatchProcessor, 1);
        }

        private class FooBatchProcessor : IBatchProcessor
        {
            public void Process(IEnumerable<Story> stories)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}