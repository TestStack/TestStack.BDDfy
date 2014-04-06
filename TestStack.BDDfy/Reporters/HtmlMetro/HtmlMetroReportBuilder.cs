using System;
using System.Linq;
using System.Text;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    public class HtmlMetroReportBuilder : IReportBuilder
    {
        private Func<DateTime> _dateProvider = () => DateTime.Now;
        private HtmlReportViewModel _viewModel;

        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportViewModel);
        }

        public string CreateReport(HtmlReportViewModel model)
        {
            _viewModel = model;

            var t = new MetroHtmlReportTemplate(model, DateProvider());

            var html = t.TransformText();

            return html;
        }

        public Func<DateTime> DateProvider
        {
            get { return _dateProvider; }
            set { _dateProvider = value; }
        }
     

    }
}
