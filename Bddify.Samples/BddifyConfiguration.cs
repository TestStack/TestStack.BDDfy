using Bddify.Configuration;
using Bddify.Reporters.HtmlReporter;
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
            Factory.ProcessorPipeline.Add(() => new CustomTextReporter());
            Factory.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig()));
        }
    }
}