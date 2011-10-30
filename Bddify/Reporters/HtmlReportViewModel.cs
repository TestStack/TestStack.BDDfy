using System.Collections.Generic;
using Bddify.Configuration;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class HtmlReportViewModel
    {
        public HtmlReportViewModel(IHtmlReportConfiguration configuration, IEnumerable<Story> stories)
        {
            Configuration = configuration;
            Stories = stories;
        }

        public IHtmlReportConfiguration Configuration { get; private set; }
        public IEnumerable<Story> Stories { get; private set; }
    }
}