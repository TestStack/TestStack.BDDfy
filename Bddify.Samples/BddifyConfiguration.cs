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
            Factory.ProcessorPipeline.HtmlReport.Configurations = new IHtmlReportConfiguration[] {new HtmlReportConfig() };
        }
    }
}