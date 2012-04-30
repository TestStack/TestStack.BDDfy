using System;
using System.Collections.Generic;
using Bddify.Core;
using Bddify.Reporters.HtmlReporter;

namespace Bddify.Configuration
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