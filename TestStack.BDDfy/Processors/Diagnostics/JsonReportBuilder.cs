using System.Linq;
using System.Text;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public class JsonReportBuilder
    {
        readonly FileReportModel _viewModel;
        private readonly StringBuilder _document;
        const int TabIndentation = 2;
        int _tabCount;

        public JsonReportBuilder(FileReportModel viewModel)
        {
            _viewModel = viewModel;
            _document = new StringBuilder();
        }

        public string BuildReport()
        {
            using (OpenTag("[", "]"))
            {
                int storyCount = 0;
                foreach (var story in _viewModel.Stories)
                {
                    storyCount++;
                    using (OpenTag("{", "}"))
                    {
                        AddJsonLine("StoryName", story.MetaData.Title);
                        AddJsonLine("StoryDuration", story.Scenarios.Sum(x => x.Duration.Milliseconds).ToString());
                        AddLine("\"Scenarios\":");
                        using(OpenTag("[", "]"))
                        {
                            int scenarioCount = 0;
                            foreach (var scenario in story.Scenarios)
                            {
                                scenarioCount++;
                                using (OpenTag("{", "}"))
                                {
                                    AddJsonLine("ScenarioName", scenario.Title);
                                    AddJsonLine("ScenarioDuration", scenario.Duration.Milliseconds.ToString());
                                    AddLine("\"Steps\":");
                                    using (OpenTag("[", "]"))
                                    {
                                        int stepCount = 0;
                                        foreach (var step in scenario.Steps)
                                        {
                                            stepCount++;
                                            using (OpenTag("{", "}"))
                                            {
                                                AddJsonLine("StepName", step.StepTitle);
                                                AddJsonLine("StepDuration", step.Duration.Milliseconds.ToString(), false);
                                            }
                                            if (stepCount < scenario.Steps.Count) _document.Append(",");
                                        }
                                    }
                                }
                                if (scenarioCount < story.Scenarios.Count()) _document.Append(",");
                            }
                        }
                    }
                    if(storyCount < _viewModel.Stories.Count()) _document.Append(",");
                }

            }
            return _document.ToString();
        }

        private BasicTag OpenTag(string openTag, string closeTag)
        {
            AddLine(openTag);
            _tabCount++;
            return new BasicTag(() => CloseTag(closeTag));
        }

        private void CloseTag(string tag)
        {
            _tabCount--;
            AddLine(tag);
        }

        private void AddLine(string line)
        {
            int tabWidth = _tabCount * TabIndentation;
            _document.AppendLine(string.Empty.PadLeft(tabWidth) + line);
        }

        private void AddJsonLine(string propertyName, string value, bool appendComma = true)
        {
            var line = string.Format("\"{0}\":{1}", propertyName, EncodeJsString(value));
            if (appendComma)
                line += ",";
            AddLine(line);
        }

        // http://www.west-wind.com/weblog/posts/2007/Jul/14/Embedding-JavaScript-Strings-from-an-ASPNET-Page
        private string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }
    }
}