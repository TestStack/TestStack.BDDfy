using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Configuration
{
    public class ProcessorPipelineTests
    {
        [Test]
        public void ReturnsDefaultPipelineByDefault()
        {
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
        }

        [Test]
        public void DoesNotReturnConsoleReportWhenItIsDeactivated()
        {
            Configurator.Processors.ConsoleReport.Disable();
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Configurator.Processors.ConsoleReport.Enable();
        }

        [Test]
        public void DoesNotReturnConsoleReportForExcludedStories()
        {
            Configurator.Processors.ConsoleReport.RunsOn(s => s.MetaData != null);
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            Assert.IsFalse(processors.Any(p => p is ConsoleReporter));
            Configurator.Processors.ConsoleReport.RunsOn(s => true);
        }

        [Test]
        public void DoesNotReturnTestRunnerWhenItIsDeactivated()
        {
            Configurator.Processors.TestRunner.Disable();
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
            Assert.IsFalse(processors.Any(p => p is TestRunner));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is ExceptionProcessor));
            Configurator.Processors.TestRunner.Enable();
        }

        [Test]
        public void CanAddCustomProcessor()
        {
            var processors = Configurator
                .Processors
                .Add(() => new CustomProcessor())
                .GetProcessors(new Story(null)).ToList();

            Assert.IsTrue(processors.Any(p => p is CustomProcessor));
            Assert.IsTrue(processors.Any(p => p is StoryCache));
            Assert.IsTrue(processors.Any(p => p is ConsoleReporter));
        }
    }
}