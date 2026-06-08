using System.Runtime.CompilerServices;
using Shouldly;
using TestStack.BDDfy.Tests.Reporters.Html;
using TextReporterClass = TestStack.BDDfy.Reporters.TextReporter;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.TextReporter
{
    [Collection("ConfiguratorState")]
    public class TextReporterWithCultureDifferentFromSutCulture
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

            using (new TemporaryCulture("tr-TR"))
            {
                var number = 0m;
                var expected = string.Empty;

                var story = this.When(_ => sut.DoSomethingWithNumber(number))
                    .Then(_ => sut.Report.ShouldBe(expected, "Report should match the expectation"))
                    .WithExamples(new ExampleTable("number", "expected")
                    {
                        { 1093.56m, "1093,56 is the final number" },
                        { 10m, "10 is the final number" }
                    })
                    .BDDfy();

                var textReporter = new TextReporterClass();
                textReporter.Process(story);
                var actualReport = textReporter.ToString();

                // The SUT uses thread culture (Turkish) so formats 1093.56 as "1093,56"
                // But BDDfy report uses Configurator.CultureInfo for step titles
                actualReport.ShouldContain("When do something with number <number>");
                actualReport.ShouldContain("| 1093.56 |");
                actualReport.ShouldContain("| 10      |");
            }
        }
    }
}
