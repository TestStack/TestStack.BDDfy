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
            return batchProcessor is HtmlReporter reporter && reporter.Configuration.ReportBuilder is MetroReportBuilder;
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
            processors.Any(p => p is GenericReporter<MarkDownReportBuilder>).ShouldBe(false);
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
            processors.Any(p => p is GenericReporter<MarkDownReportBuilder>).ShouldBe(true);

            Configurator.BatchProcessors.MarkDownReport.Disable();
        }

        [Fact]
        public void ReturnsHtmlMetroReporterWhenItIsActivatedAndConfiguredWithMetroReportBuilder()
        {
            Configurator.BatchProcessors.Configure<HtmlReporter>(reporter =>
            {
                reporter.Configuration = new HtmlReportConfiguration
                {
                    ReportBuilder = new MetroReportBuilder()
                };
            });

            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            processors.Any(MetroReportProcessorIsActive).ShouldBe(true);
            Configurator.BatchProcessors.Configure<HtmlReporter>(reporter =>
            {
                reporter.Configuration = new HtmlReportConfiguration
                {
                    ReportBuilder = new ClassicReportBuilder()
                };
            });
        }

        [Fact]
        public void ReturnsDianosticsReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.DiagnosticsReport.Enable();

            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            processors.ShouldContain(p=> p is GenericReporter<DiagnosticsReportBuilder>, 1);

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