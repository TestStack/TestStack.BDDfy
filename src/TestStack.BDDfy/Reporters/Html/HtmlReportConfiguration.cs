using TestStack.BDDfy.Reporters.Readers;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportConfiguration: ReportConfiguration<ClassicReportBuilder>
    {
        public HtmlReportConfiguration() : base("BDDfyReport.html") { }

        public bool ResolveJqueryFromCdn { get; set; } = true;
        public IFileReader FileReader { get; set; } = new FileReader();
    }
}