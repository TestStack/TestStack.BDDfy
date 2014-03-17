using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters.Html;
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
            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig()));
        }
    }
}