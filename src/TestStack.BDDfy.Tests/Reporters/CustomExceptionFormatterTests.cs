using System.Text;
using TestStack.BDDfy.Configuration;
using Xunit;
using Xunit.v3;
using TextReporterClass = TestStack.BDDfy.Reporters.TextReporter;

namespace TestStack.BDDfy.Tests.Reporters
{
    [Collection("ConfiguratorState")]
    public class CustomExceptionFormatterTests(ITestOutputHelper output)
    {
        [Fact]
        public void ShouldFormatExceptionsUsingCustomFormatter()
        {
            var original = Configurator.ExceptionFormatter;
            try
            {
                Configurator.ExceptionFormatter = new CustomExceptionFormatter();

                var stories = new ReportTestData().CreateMixContainingEachTypeOfOutcomeWithOneScenarioPerStory();
                var actual = new StringBuilder();

                foreach (var story in stories)
                {
                    var textReporter = new TextReporterClass();
                    textReporter.Process(story);
                    actual.AppendLine(textReporter.ToString());
                }

                var report = actual.ToString();
                output.WriteLine(report);

                Assert.DoesNotContain("at TestStack.BDDfy", report);
                Assert.Contains("Boom", report);
            }
            finally
            {
                Configurator.ExceptionFormatter = original;
            }
        }
    }
}
