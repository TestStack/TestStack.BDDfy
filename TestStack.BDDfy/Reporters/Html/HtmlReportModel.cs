using System.Collections.Generic;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportModel : FileReportModel
    {
        public HtmlReportModel(IHtmlReportConfiguration configuration, ReportModel reportModel)
            : base(reportModel)
        {
            Configuration = configuration;
        }

        public HtmlReportModel(ReportModel reportModel) 
            :this(new DefaultHtmlReportConfiguration(), reportModel)
        {
        }

        public string CustomStylesheet { get; set; }
        public string CustomJavascript { get; set; }

        public IHtmlReportConfiguration Configuration { get; private set; }
    }
}