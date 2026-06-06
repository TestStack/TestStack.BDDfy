using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.Serializers;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class JsonDataFileReporter(ReportConfiguration reportConfiguration, bool javascriptFormat): IBatchProcessor
    {  
        private const string DefaultOutputFileName = "bddfy-stories.json";
        private static readonly CustomJsonSerializer Serializer = new();
        public JsonDataFileReporter(bool javascriptFormat): this(DefaultReportConfiguration, javascriptFormat) { }
        public JsonDataFileReporter(): this(DefaultReportConfiguration, true) { }

        public void Process(IEnumerable<Story> stories)
        {
            var viewModel = new FileReportModel(stories.ToReportModel());
            var content = Serializer.Serialize(viewModel);
            var filename = reportConfiguration.OutputFileName;
            
            if (javascriptFormat)
            {
                content = $"var STORIES_DATA = {content}";
                filename = filename.Replace(".json", ".js");
            }

            reportConfiguration.FileWriter.OutputReport(content, filename);
        }

        private static ReportConfiguration DefaultReportConfiguration => new(DefaultOutputFileName);
    }
}
