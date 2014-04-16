namespace TestStack.BDDfy.Reporters.Html
{
    public class MetroReportBuilder : IReportBuilder
    {
        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportModel);
        }

        public string CreateReport(HtmlReportModel model)
        {
            return new MetroReportTemplate(model).TransformText();
        }
    }
}
