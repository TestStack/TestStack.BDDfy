namespace TestStack.BDDfy.Reporters.Html
{
    public class ModernReportBuilder: IReportBuilder
    {
        public string CreateReport(FileReportModel model) => HtmlReportResources.modern_html_report;
    }
}
