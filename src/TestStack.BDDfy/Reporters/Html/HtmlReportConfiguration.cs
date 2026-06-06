using TestStack.BDDfy.Reporters.Readers;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportConfiguration: ReportConfiguration<ClassicReportBuilder>
    {
        public HtmlReportConfiguration() : base("bddfy-report.html") {
            ReportBuilder = new ModernReportBuilder();
        }
       
        public bool ResolveJqueryFromCdn { get; set; } = true;
        public IFileReader FileReader { get; set; } = new FileReader();
    }
}