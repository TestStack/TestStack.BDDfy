namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportModel(HtmlReportConfiguration configuration, ReportModel reportModel): FileReportModel(reportModel)
    {
        public HtmlReportModel(ReportModel reportModel) : this(new HtmlReportConfiguration(), reportModel)
        {
        }

        public string? CustomStylesheet { get; set; }
        public string? CustomJavascript { get; set; }

        public HtmlReportConfiguration Configuration { get; private set; } = configuration;
    }
}