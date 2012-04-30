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
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Assert.IsFalse(processors.Any(p => p is MarkDownReporter));
        }

        [Test]
        public void DoesNotReturnConsoleReportWhenItIsDeactivated()
        {
            Factory.ConsoleReport.Disable();
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.ConsoleReport.Enable();
        }

        [Test]
        public void DoesNotReturnConsoleReportForExcludedStories()
        {
            Factory.ConsoleReport.RunsOn(s => s.MetaData != null);
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Factory.ConsoleReport.RunsOn(s => true);
        }

        [Test]
        public void DoesNotReturnHtmlReporterWhenItIsDeactivated()
        {
            Factory.HtmlReport.Disable();
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.HtmlReport.Enable();
        }

        [Test]
        public void DoesNotReturnTestRunnerWhenItIsDeactivated()
        {
            Factory.TestRunner.Disable();
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.TestRunner.Enable();
        }

        [Test]
        public void DoesNotReturnHtmlReporterForExcludedStories()
        {
            Factory.HtmlReport.RunsOn(s => s.MetaData != null);
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is HtmlReporter));
            Factory.HtmlReport.RunsOn(s => true);
        }

        [Test]
        public void ReturnsMarkdownReporterWhenItIsActivated()
        {
            Factory.MarkdownReport.Enable();
            var processors = Factory.Pipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is MarkDownReporter));
            Factory.MarkdownReport.Disable();
        }

        [Test]
        public void CanAddCustomProcessor()
        {
            var processors = Factory
                .Pipeline
                .Add(() => new CustomProcessor())
                .GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is CustomProcessor));
            Assert.IsTrue(processors.Any(p => p is HtmlReporter));
            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
        }
    }
}