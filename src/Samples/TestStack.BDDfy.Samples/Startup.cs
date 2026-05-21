using System.Runtime.CompilerServices;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Samples.Atm;

namespace TestStack.BDDfy.Samples
{
    public class Startup
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            Configurator.Processors.Add(() => new CustomTextReporter());
            Configurator.Processors.ConsoleReport.Enable();
            Configurator.BatchProcessors.MarkDownReport.Enable();
            Configurator.BatchProcessors.DiagnosticsReport.Enable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new AtmHtmlReportConfig(), new MetroReportBuilder()));
        }
    }
}