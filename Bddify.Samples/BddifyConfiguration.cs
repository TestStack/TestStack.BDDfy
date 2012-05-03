using Bddify.Configuration;
using Bddify.Core;
using Bddify.Processors.HtmlReporter;
using Bddify.Samples.Atm;
using Bddify.Samples.TicTacToe;
using NUnit.Framework;

namespace Bddify.Samples
{
    [SetUpFixture]
    public class BddifyConfiguration
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