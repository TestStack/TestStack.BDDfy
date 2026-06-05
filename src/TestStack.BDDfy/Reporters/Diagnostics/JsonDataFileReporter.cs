using TestStack.BDDfy.Reporters.Serializers;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class JsonDataFileReporter: IBatchProcessor
    {
        private static readonly CustomJsonSerializer Serializer = new();
        private static readonly FileWriter Writer = new ();
        public void Process(IEnumerable<Story> stories)
        {
            var viewModel = new FileReportModel(stories.ToReportModel());
            var json = Serializer.Serialize(viewModel);
            Writer.OutputReport($"var STORIES_DATA = {json}", "stories.js");
        }
    }
}
