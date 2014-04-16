namespace TestStack.BDDfy.Reporters.Html
{
    /// <summary>
    /// Partial class for the T4 runtime template where it's data comes from
    /// </summary>
    partial class MetroReportTemplate
    {
        private readonly HtmlReportModel _model;

        public MetroReportTemplate(HtmlReportModel model)
        {
            _model = model;
        }

        public HtmlReportModel Model
        {
            get { return _model; }
        }

        public string ReportCss
        {
            get
            {
                return HtmlReportResources.metro_css_min;
            }
        }

        public string ReportJs
        {
            get
            {
                return HtmlReportResources.metro_js_min;
            }
        }
    }
}