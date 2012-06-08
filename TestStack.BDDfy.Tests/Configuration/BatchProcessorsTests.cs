using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Processors.HtmlReporter;

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
    }
}