using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Configuration
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class ProcessorPipelineTests
    {
        [Fact]
        public void ReturnsDefaultPipelineByDefault()
        {
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            processors.Any(p => p is ConsoleReporter).ShouldBe(true);
            processors.Any(p => p is StoryCache).ShouldBe(true);
            processors.Any(p => p is TestRunner).ShouldBe(true);
            processors.Any(p => p is ExceptionProcessor).ShouldBe(true);
        }

        [Fact]
        public void DoesNotReturnConsoleReportWhenItIsDeactivated()
        {
            Configurator.Processors.ConsoleReport.Disable();
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            processors.Any(p => p is ConsoleReporter).ShouldBe(false);
            processors.Any(p => p is StoryCache).ShouldBe(true);
            processors.Any(p => p is TestRunner).ShouldBe(true);
            processors.Any(p => p is ExceptionProcessor).ShouldBe(true);
            Configurator.Processors.ConsoleReport.Enable();
        }

        [Fact]
        public void DoesNotReturnConsoleReportForExcludedStories()
        {
            Configurator.Processors.ConsoleReport.RunsOn(s => s.Metadata != null);
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            processors.Any(p => p is ConsoleReporter).ShouldBe(false);
            Configurator.Processors.ConsoleReport.RunsOn(s => true);
        }

        [Fact]
        public void DoesNotReturnTestRunnerWhenItIsDeactivated()
        {
            Configurator.Processors.TestRunner.Disable();
            var processors = Configurator.Processors.GetProcessors(new Story(null)).ToList();

            processors.Any(p => p is ConsoleReporter).ShouldBe(true);
            processors.Any(p => p is TestRunner).ShouldBe(false);
            processors.Any(p => p is StoryCache).ShouldBe(true);
            processors.Any(p => p is ExceptionProcessor).ShouldBe(true);
            Configurator.Processors.TestRunner.Enable();
        }

        [Fact]
        public void CanAddCustomProcessor()
        {
            var processors = Configurator
                .Processors
                .Add(() => new CustomProcessor())
                .GetProcessors(new Story(null)).ToList();

            processors.Any(p => p is CustomProcessor).ShouldBe(true);
            processors.Any(p => p is StoryCache).ShouldBe(true);
            processors.Any(p => p is ConsoleReporter).ShouldBe(true);
        }
    }
}