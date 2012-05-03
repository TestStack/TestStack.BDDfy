using System;
using Bddify.Configuration;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using Bddify.Reporters.HtmlReporter;
using Bddify.Reporters.MarkDownReporter;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Configuration
{
    [TestFixture]
    public class BatchProcessorsTests
    {
        [Test]
        public void ReturnsHtmlReporterByDefault()
        {
            var processors = Factory.BatchProcessors.GetProcessors().ToList();
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
        }

        [Test]
        public void DoesNotReturnMarkDownReporterByDefault()
        {
            var processors = Factory.BatchProcessors.GetProcessors().ToList();
            Assert.IsFalse(processors.Any(p => p is MarkDownReporter));
        }

        [Test]
        public void DoesNotReturnHtmlReporterWhenItIsDeactivated()
        {
            Factory.BatchProcessors.HtmlReporter.Disable();
            var processors = Factory.BatchProcessors.GetProcessors().ToList();

            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Factory.BatchProcessors.HtmlReporter.Enable();
        }

        [Test]
        public void ReturnsMarkdownReporterWhenItIsActivated()
        {
            Factory.BatchProcessors.MarkDownReport.Enable();
            var processors = Factory.BatchProcessors.GetProcessors().ToList();

            Assert.IsTrue(processors.Any(p => p is MarkDownReporter));
            Factory.BatchProcessors.MarkDownReport.Disable();
        }
    }
}