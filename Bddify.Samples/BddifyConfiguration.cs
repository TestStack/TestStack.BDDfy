using Bddify.Core;
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
            Configurator.Pipeline
                .Add(() => new CustomTextReporter())
                .RunConsoleReportOnlyWhen(s => s.Result == StepExecutionResult.Failed);

            Configurator.HtmlReportConfigurations = new IHtmlReportConfiguration[] {new HtmlReportConfig() };
        }
    }
}