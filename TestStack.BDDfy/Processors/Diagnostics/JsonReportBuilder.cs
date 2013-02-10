using System.Collections.Generic;
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
                        AddLine("[");

                        int scenarioCount = 0;
                        foreach (var scenario in story.Scenarios)
                        {
                            scenarioCount++;
                            using (OpenTag("{", "}"))
                            {
                                AddJsonLine("ScenarioName", scenario.Title);
                                AddJsonLine("ScenarioDuration", scenario.Duration.Milliseconds.ToString());
                                AddLine("\"Steps\":");

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
                            if(scenarioCount < story.Scenarios.Count()) _document.Append(",");
                        }
                        AddLine("]");
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
            var line = string.Format("\"{0}\":\"{1}\"", propertyName, value);
            if (appendComma)
                line += ",";
            AddLine(line);
        }
    }
}