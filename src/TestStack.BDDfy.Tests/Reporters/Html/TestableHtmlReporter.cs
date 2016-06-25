#if NSubstitute
using NSubstitute;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.Readers;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class TestableHtmlReporter : HtmlReporter
    {
        public IHtmlReportConfiguration Configuration { get; set; }
        public IReportWriter Writer { get; set; }
        public IFileReader FileReader { get; set; }

        public TestableHtmlReporter(IHtmlReportConfiguration configuration, IReportBuilder reportBuilder, IReportWriter writer, IFileReader fileReader) 
            : base(configuration, reportBuilder, writer, fileReader)
        {
            Configuration = configuration;
            ReportBuilder = reportBuilder;
            Writer = writer;
            FileReader = fileReader;
            Configuration.RunsOn(Arg.Any<Story>()).Returns(true);
        }

        public static TestableHtmlReporter Create()
        {
            return new TestableHtmlReporter(
                Substitute.For<IHtmlReportConfiguration>(), Substitute.For<IReportBuilder>(), 
                Substitute.For<IReportWriter>(), Substitute.For<IFileReader>());
        }
    }
}
#endif