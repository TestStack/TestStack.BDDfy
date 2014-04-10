using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.HtmlMetro;
using TestStack.BDDfy.Samples.Atm;

namespace TestStack.BDDfy.Samples
{
    [SetUpFixture]
    public class BDDfyConfiguration
    {
        [SetUp]
        public void Config()
        {
            Configurator.Processors.Add(() => new CustomTextReporter());
            Configurator.BatchProcessors.MarkDownReport.Enable();
            Configurator.BatchProcessors.DiagnosticsReport.Enable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new AtmHtmlReportConfig(), new HtmlMetroReportBuilder()));
        }
    }
}