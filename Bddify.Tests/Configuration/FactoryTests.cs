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
    public class FactoryTests
    {
        [Test]
        public void ReturnsDefaultPipelineByDefault()
        {
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Assert.IsFalse(processors.Any(p => p is MarkDownReporter));
        }

        [Test]
        public void DoesNotReturnConsoleReportWhenItIsDeactivated()
        {
            Factory.ProcessorPipeline.ConsoleReport.Disable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.ProcessorPipeline.ConsoleReport.Enable();
        }

        [Test]
        public void DoesNotReturnConsoleReportForExcludedStories()
        {
            Factory.ProcessorPipeline.ConsoleReport.RunsOn(s => s.MetaData != null);
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Factory.ProcessorPipeline.ConsoleReport.RunsOn(s => true);
        }

        [Test]
        public void DoesNotReturnHtmlReporterWhenItIsDeactivated()
        {
            Factory.ProcessorPipeline.HtmlReport.Disable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.ProcessorPipeline.HtmlReport.Enable();
        }

        [Test]
        public void DoesNotReturnTestRunnerWhenItIsDeactivated()
        {
            Factory.ProcessorPipeline.TestRunner.Disable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.ProcessorPipeline.TestRunner.Enable();
        }

        [Test]
        public void DoesNotReturnHtmlReporterForExcludedStories()
        {
            Factory.ProcessorPipeline.HtmlReport.RunsOn(s => s.MetaData != null);
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Factory.ProcessorPipeline.HtmlReport.RunsOn(s => true);
        }

        [Test]
        public void ReturnsMarkdownReporterWhenItIsActivated()
        {
            Factory.ProcessorPipeline.MarkdownReport.Enable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is MarkDownReporter));
            Factory.ProcessorPipeline.MarkdownReport.Disable();
        }

        [Test]
        public void CanAddCustomProcessor()
        {
            var processors = Factory
                .ProcessorPipeline
                .Add(() => new CustomProcessor())
                .GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is CustomProcessor));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
        }
    }
}