using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Samples.Atm
{
    public class AtmHtmlReportConfig : HtmlReportConfiguration
    {
        public AtmHtmlReportConfig()
        {
            ReportHeader = "ATM Solutions";
            OutputFileName = "ATM.html";
            ReportDescription = "A reliable solution for your offline banking needs";
            ResolveJqueryFromCdn = false;
        }

        public override bool RunsOn(Story story) => story.Namespace.EndsWith("Atm", StringComparison.OrdinalIgnoreCase);
    }
}