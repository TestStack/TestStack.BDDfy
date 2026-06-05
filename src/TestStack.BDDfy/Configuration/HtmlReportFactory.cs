using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Configuration
{
    public class HtmlReportFactory : ProcessorFactory
    {
        internal HtmlReportFactory(Func<IProcessor> factory) : base(factory)
        {
        }

        public IEnumerable<HtmlReportConfiguration> Configurations = [new HtmlReportConfiguration()];    
    }
}