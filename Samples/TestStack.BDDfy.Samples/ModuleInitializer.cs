using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Samples;
using TestStack.BDDfy.Samples.Atm;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        Configurator.Processors.Add(() => new CustomTextReporter());
        Configurator.BatchProcessors.MarkDownReport.Enable();
        Configurator.BatchProcessors.DiagnosticsReport.Enable();
        Configurator.BatchProcessors.Add(new HtmlReporter(new AtmHtmlReportConfig(), new MetroReportBuilder()));
    }
}