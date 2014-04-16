using System;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    public class HtmlMetroReportBuilder : IReportBuilder
    {
        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportModel);
        }

        public string CreateReport(HtmlReportModel model)
        {
            return new MetroHtmlReportTemplate(model).TransformText();
        }
    }
}
