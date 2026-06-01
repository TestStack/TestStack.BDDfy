using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Shouldly;
using TestStack.BDDfy.Configuration;
using TextReporterClass = TestStack.BDDfy.Reporters.TextReporter;
using Xunit;
using System.Runtime.Serialization;

namespace TestStack.BDDfy.Tests.Reporters.TextReporter
{
    public class TextReporterWithTurkishCultureTests
    {

        private class MySut
        {
            public string Report = string.Empty;
            public void DoSomethingWithNumber(decimal number) => Report = $"{number} is the final number";
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ReportUsesConfiguratorCultureRegardlessOfThreadCulture()
        {
            var sut = new MySut();
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalReporterCulture = Configurator.CultureInfo;
            try
            {
                var number = 0m;
                var expected = string.Empty;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
                Configurator.CultureInfo = originalCulture;
                // Act
                var story = this.When(_ => sut.DoSomethingWithNumber(number))
                    .Then(_ => sut.Report.ShouldBe(expected, "Report should match the expectation"))
                    .WithExamples(new ExampleTable("number","expected")
                    {
                          { 1093.56m, "1093,56 is the final number" },
                          { 10m, "10 is the final number" }
                    })
                    .BDDfy();

                var textReporter = new TextReporterClass();
                textReporter.Process(story);
                var actualReport = textReporter.ToString();

                // The SUT uses thread culture (Turkish) so formats 1093.56 as "1093,56"
                // But BDDfy report uses Configurator.Culture for step titles
                actualReport.ShouldContain("When do something with number <number>");
                actualReport.ShouldContain("| 1093.56 |");
                actualReport.ShouldContain("| 10      |");
            }
            finally
            {                       
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Configurator.CultureInfo = originalReporterCulture;
            }
        }
    }
}
