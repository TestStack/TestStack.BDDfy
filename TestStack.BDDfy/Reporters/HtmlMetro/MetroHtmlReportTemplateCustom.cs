using System;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    partial class MetroHtmlReportTemplate
    {
        private readonly HtmlReportViewModel _model;      
        public DateTime RunDate { get; set; }

        public MetroHtmlReportTemplate(HtmlReportViewModel model, DateTime runDate)
        {
            _model = model;
            RunDate = runDate;
        }

        public HtmlReportViewModel Model
        {
            get { return _model; }
        }
    }
}