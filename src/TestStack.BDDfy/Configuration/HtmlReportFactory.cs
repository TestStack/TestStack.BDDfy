using System;
using System.Collections.Generic;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Configuration
{
    public class HtmlReportFactory : ProcessorFactory
    {
        internal HtmlReportFactory(Func<IProcessor> factory) : base(factory)
        {
        }

        public IEnumerable<IHtmlReportConfiguration> Configurations =
            new IHtmlReportConfiguration[]
                {
                    new DefaultHtmlReportConfiguration()
                };
    }
}