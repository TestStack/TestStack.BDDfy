using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class HtmlReportViewModel
    {
        public HtmlReportViewModel(IHtmlReportConfigurationModule configuration, IEnumerable<Story> stories)
        {
            Configuration = configuration;
            Stories = stories;
        }

        public IHtmlReportConfigurationModule Configuration { get; private set; }
        public IEnumerable<Story> Stories { get; private set; }
    }
}