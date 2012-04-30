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
            Factory.Pipeline.Add(() => new CustomTextReporter());
            Factory.Scanner.HtmlReportConfigurations = new IHtmlReportConfiguration[] {new HtmlReportConfig() };
        }
    }
}