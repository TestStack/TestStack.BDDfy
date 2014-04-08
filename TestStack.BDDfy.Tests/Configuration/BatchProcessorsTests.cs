using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.HtmlMetro;
using TestStack.BDDfy.Reporters.MarkDown;

namespace TestStack.BDDfy.Tests.Configuration
{
    [TestFixture]
    public class BatchProcessorsTests
    {
        [Test]
        public void ReturnsHtmlReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
        }

        [Test]
        public void DoesNotReturnMarkDownReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            Assert.IsFalse(processors.Any(p => p is MarkDownReporter));
        }

        [Test]
        public void DoesNotReturnHtmlMetroReporterByDefault()
        {
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();
            Assert.IsFalse(processors.Any(p => p is HtmlMetroReporter));
        }

        [Test]
        public void DoesNotReturnHtmlReporterWhenItIsDeactivated()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Configurator.BatchProcessors.HtmlReport.Enable();
        }

        [Test]
        public void ReturnsMarkdownReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.MarkDownReport.Enable();
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            Assert.IsTrue(processors.Any(p => p is MarkDownReporter));
            Configurator.BatchProcessors.MarkDownReport.Disable();
        }

        [Test]
        public void ReturnsHtmlMetroReporterWhenItIsActivated()
        {
            Configurator.BatchProcessors.HtmlMetroReport.Enable();           
            
            var processors = Configurator.BatchProcessors.GetProcessors().ToList();

            Assert.IsTrue(processors.OfType<HtmlMetroReporter>().Any(),
                "The metro Html report was not found in batch processors");

            Configurator.BatchProcessors.HtmlMetroReport.Disable();
        }
    }
}