using System.Linq;
using Bddify.Configuration;
using Bddify.Processors;
using Bddify.Reporters.ConsoleReporter;
using NUnit.Framework;

namespace Bddify.Tests.Configuration
{
    public class ProcessorPipelineTests
    {
        [Test]
        public void ReturnsDefaultPipelineByDefault()
        {
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
        }

        [Test]
        public void DoesNotReturnConsoleReportWhenItIsDeactivated()
        {
            Factory.ProcessorPipeline.ConsoleReport.Disable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
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
        public void DoesNotReturnTestRunnerWhenItIsDeactivated()
        {
            Factory.ProcessorPipeline.TestRunner.Disable();
            var processors = Factory.ProcessorPipeline.GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Factory.ProcessorPipeline.TestRunner.Enable();
        }

        [Test]
        public void CanAddCustomProcessor()
        {
            var processors = Factory
                .ProcessorPipeline
                .Add(() => new CustomProcessor())
                .GetProcessors(new Core.Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is CustomProcessor));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
        }
    }
}