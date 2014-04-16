using System.Collections.Generic;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReportViewModel : FileReportModel
    {
        public HtmlReportViewModel(IHtmlReportConfiguration configuration, IEnumerable<Story> stories)
            : base(stories)
        {
            Configuration = configuration;
        }

        public HtmlReportViewModel(IEnumerable<Story> stories) 
            :this(new DefaultHtmlReportConfiguration(), stories)
        {
        }

        public string CustomStylesheet { get; set; }
        public string CustomJavascript { get; set; }

        public IHtmlReportConfiguration Configuration { get; private set; }
    }
}