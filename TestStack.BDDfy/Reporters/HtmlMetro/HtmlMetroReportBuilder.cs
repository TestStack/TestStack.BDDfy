using System;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    public class HtmlMetroReportBuilder : IReportBuilder
    {
        private Func<DateTime> _dateProvider = () => DateTime.Now;

        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportViewModel);
        }

        public string CreateReport(HtmlReportViewModel model)
        {
            return new MetroHtmlReportTemplate(model, DateProvider()).TransformText();
        }

        public Func<DateTime> DateProvider
        {
            get { return _dateProvider; }
            set { _dateProvider = value; }
        }     
    }
}
