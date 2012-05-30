using BDDfy.Configuration;
using BDDfy.Processors.HtmlReporter;
using BDDfy.Samples.Atm;
using BDDfy.Samples.TicTacToe;
using NUnit.Framework;

namespace BDDfy.Samples
{
    [SetUpFixture]
    public class BDDfyConfiguration
    {
        [SetUp]
        public void Config()
        {
            Configurator.Processors.Add(() => new CustomTextReporter());
            Configurator.BatchProcessors.MarkDownReport.Enable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig()));
        }
    }
}