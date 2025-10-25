using System.Collections.Generic;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportModel(IHtmlReportConfiguration configuration, ReportModel reportModel): FileReportModel(reportModel)
    {
        public HtmlReportModel(ReportModel reportModel) 
            :this(new DefaultHtmlReportConfiguration(), reportModel)
        {
        }

        public string CustomStylesheet { get; set; }
        public string CustomJavascript { get; set; }

        public IHtmlReportConfiguration Configuration { get; private set; } = configuration;
    }
}