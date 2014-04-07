using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.HtmlMetro;
using TestStack.BDDfy.Reporters.Readers;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Tests.Reporters.HtmlMetro
{
    /// <summary>
    /// Wasn't sure where/if to put this test
    /// </summary>
    [TestFixture]
    public class ConfiguredHtmlMetroReportTests
    {
        [Test]
        public void ConfiguringTheHtmlMetroReporterRunsWithoutError()
        {
            Configurator.BatchProcessors.Add(new HtmlReporter(new DefaultHtmlReportConfiguration(), new HtmlMetroReportBuilder(), new FileWriter(),
                new FileReader()));

            Configurator.BatchProcessors.HtmlReport.Disable();

            this.Given(x => GivenA())
                    .When(x => WhenB())
                    .Then(x => ThenC())
                .BDDfy();
        }


        private void GivenA()
        {
        }

        private void WhenB()
        {
        }
        
        private void ThenC()
        {
        }
    }
}