using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class StoryConfig
    {
        public StoryConfig(IHtmlReportConfigurationModule configurationModule, List<Story> stories)
        {
            HtmlReportConfigurationModule = configurationModule;
            Stories = stories;
        }

        public IHtmlReportConfigurationModule HtmlReportConfigurationModule { get; set; }

        public List<Story> Stories { get; set; }
    }
}
