// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Linq;
using System.Text;
using Bddify.Core;

namespace Bddify.Reporters
{
    public class HtmlReportBuilder
    {
        readonly HtmlReportViewModel _viewModel;
        readonly StringBuilder _html;
        const int _tabIndentation = 2;
        int _tabCount = 0;

        public HtmlReportBuilder(HtmlReportViewModel viewModel)
        {
            _viewModel = viewModel;
            _html = new StringBuilder();
        }

        public string BuildReportHtml()
        {
            AddLine("<!DOCTYPE html>");
            OpenTag("<html>");

            Header();
            Body();

            CloseTag("</html");

            return _html.ToString();
        }

        private void Header()
        {
            OpenTag("<head>");
            AddLine("<meta charset=\"utf-8\"/>");
            AddLine("<link href=\"bddify.css\" rel=\"stylesheet\"/>");
            AddLine("<script type=\"text/javascript\" src=\"jquery-1.7.1.min.js\"></script>");
            AddLine(string.Format("<title>Bddify Test Result {0}</title>", DateTime.Now.ToShortDateString()));
            CloseTag("</head>");
        }

        private void Body()
        {
            OpenTag("<body>");
            OpenTag("<div id=\"main\">");

            BodyHeaderAndResults();
            ExpandCollapse();
            BodyStories();

            CloseTag("</div>");     // End div id="main"

            Footer();
            CloseTag("</body>");
        }

        private void BodyHeaderAndResults()
        {
            OpenTag("<header>");
            AddLine(string.Format("<div id=\"bddifyTitle\">{0}</div>", _viewModel.Configuration.ReportHeader));
            AddLine(string.Format("<div id=\"bddifyDescription\">{0}</div>", _viewModel.Configuration.ReportDescription));
            CloseTag("</header>");

            OpenTag("<section class=\"summary\">");
            OpenTag("<ul class=\"resultSummary\">");

            AddResultListItem("namespace", "Namespaces", _viewModel.Results.Namespaces);
            AddResultListItem("story", "Stories", _viewModel.Results.Stories);
            AddResultListItem("Passed", "Passed", _viewModel.Results.Passed);
            AddResultListItem("Failed", "Failed", _viewModel.Results.Failed);
            AddResultListItem("Inconclusive", "Inconclusive", _viewModel.Results.Inconclusive);
            AddResultListItem("NotImplemented", "Not Implemented", _viewModel.Results.NotImplemented);
            AddResultListItem("NotExecuted", "Not Executed", _viewModel.Results.NotExecuted);

            CloseTag("</ul>");
            CloseTag("</section>");
        }

        private void ExpandCollapse()
        {
            OpenTag("<p>");
            AddLine("<span id=\"expandAll\">expand all</span>");
            AddLine("<span id=\"collapseAll\" class=\"NotExecuted\">collapse all</span>");
            CloseTag("</p>");
        }

        private void BodyStories()
        {
            OpenTag("<div id=\"testResult\">");
            OpenTag("<ul class=\"testResult\">");

            foreach (var scenarioGroup in _viewModel.GroupedScenarios)
            {
                AddStory(scenarioGroup);
            }

            CloseTag("</ul>");
            AddLine(string.Format("<p><span>Tested at: {0}</span></p>", DateTime.Now));
            CloseTag("</div>");
        }

        private void Footer()
        {
            string footer = @"    <footer>Powered by <a href='https://code.google.com/p/bddify/'>bddify</a> framework</footer>
		<script type='text/javascript'>
		$(document).ready(function() {
			$('#expandAll').click(function() {
				$('.steps').css('display', '');
			});
			$('#collapseAll').click(function() {
				$('.steps').css('display', 'none');
			});
		});
		  function toggle(id) {
		    var e = document.getElementById(id);
		    if (e.style.display == 'none') {
		      e.style.display = '';
		    }
		    else {
		      e.style.display = 'none';
		    }
		  }
		</script>";
            _html.AppendLine(footer);
        }

        private void OpenTag(string tagName)
        {
            AddLine(tagName);
            _tabCount++;
        }

        private void CloseTag(string tagName)
        {
            _tabCount--;
            AddLine(tagName);
        }

        private void AddLine(string line)
        {
            int tabWidth = _tabCount * _tabIndentation;
            _html.AppendLine(string.Empty.PadLeft(tabWidth) + line);
        }

        private void AddResultListItem(string cssClass, string label, int count)
        {
            OpenTag(string.Format("<li class=\"{0}\">", cssClass));
            OpenTag("<div class=\"summary\">");
            AddLine(string.Format("<div class=\"summaryLabel\">{0}</div>", label));
            AddLine(string.Format("<span class=\"summaryCount\">{0}</span>", count));
            CloseTag("</div>");
            CloseTag("</li>");
        }

        private void AddStory(IGrouping<string, Story> scenarioGroup)
        {
            var story = scenarioGroup.First();
            var scenariosInGroup = scenarioGroup.SelectMany(s => s.Scenarios);
            var storyResult = (StepExecutionResult)scenariosInGroup.Max(s => (int)s.Result);

            OpenTag("<li>");
            OpenTag(string.Format("<div class=\"story {0}\">", storyResult));

            AddStoryMetaDataAndNarrative(story);

            OpenTag("<div class=\"scenarios\">");
            foreach (var scenario in scenariosInGroup)
            {
                AddScenario(scenario);

            }
            CloseTag("</div>");       // End of scenarios div

            CloseTag("</div>");       // End of story div
            CloseTag("</li>");
        }

        private void AddScenario(Scenario scenario)
        {
            OpenTag(string.Format("<div class=\"scenario {0}\" onclick=\"toggle('{1}');\">", scenario.Result, scenario.Id));
            OpenTag(string.Format("<div class=\"scenarioTitle\">{0}</div>", scenario.ScenarioText));

            OpenTag(string.Format("<ul class=\"steps\" id=\"{0}\" style=\"display:none\">", scenario.Id));

            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
            {
                string stepClass = string.Empty;
                string result = step.StepTitle;
                var reportException = step.Exception != null && step.Result == StepExecutionResult.Failed;
                if (reportException)
                {
                    stepClass = step.Result + "Exception";
                    if (!string.IsNullOrEmpty(step.Exception.Message))
                    {
                        result += " [Exception Message: '" + step.Exception.Message + "']";
                    }
                }

                OpenTag(string.Format("<li class=\"step {0} {1} {2}\" onclick=\"toggle('{3}');\">",step.Result, stepClass, step.ExecutionOrder, step.Id));
                AddLine(string.Format("<span>{0}</span>", result));
                if (reportException)
                {
                    OpenTag(string.Format("<div class=\"step {0}\" id=\"{1}\">",stepClass, step.Id));
                    AddLine(string.Format("<code>{0}</code>", step.Exception.StackTrace));
                    CloseTag("</div>");
                }
                CloseTag("</li>");
            }


            CloseTag("</ul>");
            CloseTag("</div>");       // End of scenario div
        }

        private void AddStoryMetaDataAndNarrative(Story story)
        {
            OpenTag("<div class=\"storyMetaData\">");

            if (story.MetaData == null)
            {
                var @namespace = story.Scenarios.First().TestObject.GetType().Namespace;
                AddLine(string.Format("div class=\"namespaceName\">{0}</div>", @namespace));
            }
            else
            {
                AddLine(string.Format("<div class=\"storyTitle\">{0}</div>", story.MetaData.Title));
            }

            if (story.MetaData != null && !string.IsNullOrEmpty(story.MetaData.AsA))
            {
                OpenTag("<ul class=\"storyNarrative\">");
                AddLine(string.Format("<li>{0}</li>", story.MetaData.AsA));
                AddLine(string.Format("<li>{0}</li>", story.MetaData.IWant));
                AddLine(string.Format("<li>{0}</li>", story.MetaData.SoThat));
                CloseTag("</ul>");
            }


            CloseTag("</div>"); // End of storyMetaData div
        }
    }
}
