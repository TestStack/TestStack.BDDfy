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
        const int _tabIndentation = 4;

        public HtmlReportBuilder(HtmlReportViewModel viewModel)
        {
            _viewModel = viewModel;
            _html = new StringBuilder();
        }

        public string BuildReportHtml()
        {
            Header();
            Body();
            Footer();

            return _html.ToString();
        }

        private void Header()
        {
            AddLine("<!DOCTYPE html>", 0);
            AddLine("<html>", 0);
            AddLine("<head>", 1);
            AddLine("<meta charset=\"utf-8\"/>", 2);
            AddLine("<link href=\"bddify.css\" rel=\"stylesheet\"/>", 2);
            AddLine("<script type=\"text/javascript\" src=\"jquery-1.7.1.min.js\"></script>", 2);
            AddLine(string.Format("<title>Bddify Test Result {0}</title>", DateTime.Now.ToShortDateString()), 2);
            AddLine("</head>", 1);
        }

        private void Body()
        {
            AddLine("<body>", 1);
            AddLine("<div id=\"main\">", 2);

            BodyHeaderAndResults();
            BodyStories();

            AddLine("</div>", 2);     // End div id="main"
            //AddLine("</body>", 1);
        }

        private void BodyHeaderAndResults()
        {
            AddLine("<header>", 3);
            AddLine(string.Format("<div id=\"bddifyTitle\">{0}</div>", _viewModel.Configuration.ReportHeader), 4);
            AddLine(string.Format("<div id=\"bddifyDescription\">{0}</div>", _viewModel.Configuration.ReportDescription), 4);
            AddLine("</header>", 3);

            AddLine("<section class=\"summary\">", 3);
            AddLine("<p><span id=\"expandAll\" class=\"NotExecuted\">expand all</span> | <span id=\"collapseAll\" class=\"NotExecuted\">collapse all</span></p>", 4);
            AddLine(string.Format("<h3>Assembly: '{0}'</h3>", _viewModel.AssemblyName), 4);
            AddLine("<ul class=\"resultSummary\">", 4);

            AddResultListItem("namespace", "Namespaces", _viewModel.Results.Namespaces);
            AddResultListItem("story", "Stories", _viewModel.Results.Stories);
            AddResultListItem("Passed", "Passed", _viewModel.Results.Passed);
            AddResultListItem("Failed", "Failed", _viewModel.Results.Failed);
            AddResultListItem("Inconclusive", "Inconclusive", _viewModel.Results.Inconclusive);
            AddResultListItem("NotImplemented", "Not Implemented", _viewModel.Results.NotImplemented);
            AddResultListItem("NotExecuted", "Not Executed", _viewModel.Results.NotExecuted);

            AddLine("</ul>", 4);
            AddLine("</section>", 3);
        }

        private void BodyStories()
        {
            AddLine("<div id=\"testResult\">", 3);
            AddLine("<ul class=\"testResult\">", 4);

            foreach (var scenarioGroup in _viewModel.GroupedScenarios)
            {
                AddStory(scenarioGroup);
            }

            AddLine("</ul>", 4);
            AddLine(string.Format("<p><span>Tested at: {0}</span></p>", DateTime.Now), 4);
            AddLine("</div>", 3);
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
		</script>
	</body>
</html>";
            _html.AppendLine(footer);
        }

        private void AddLine(string line, int tabCount)
        {
            int tabWidth = tabCount * _tabIndentation;
            _html.AppendLine(line.PadLeft(tabWidth));
        }

        private void AddResultListItem(string cssClass, string label, int count)
        {
            AddLine(string.Format("<li class=\"{0}\">", cssClass), 5);
            AddLine("<div class=\"summary\">", 6);
            AddLine(string.Format("<div class=\"summaryLabel\">{0}</div>", label), 7);
            AddLine(string.Format("<span class=\"summaryCount\">{0}</span>", count), 7);
            AddLine("</div>", 6);
            AddLine("</li>", 5);
        }

        private void AddStory(IGrouping<string, Story> scenarioGroup)
        {
            var story = scenarioGroup.First();
            var scenariosInGroup = scenarioGroup.SelectMany(s => s.Scenarios);
            var storyResult = (StepExecutionResult)scenariosInGroup.Max(s => (int)s.Result);

            AddLine("<li>", 5);
            AddLine(string.Format("<div class=\"story {0}\">", storyResult), 6);

            AddStoryMetaDataAndNarrative(story);

            AddLine("<div class=\"scenarios\">", 7);
            foreach (var scenario in scenariosInGroup)
            {
                AddScenario(scenario);

            }
            AddLine("</div>", 7);       // End of scenarios div

            AddLine("</div>", 6);       // End of story div
            AddLine("</li>", 5);
        }

        private void AddScenario(Scenario scenario)
        {
            AddLine(string.Format("<div class=\"scenario {0}\" onclick=\"toggle('{1}');\">", scenario.Result, scenario.Id), 8);
            AddLine(string.Format("<div class=\"scenarioTitle\">{0}</div>", scenario.ScenarioText), 9);

            AddLine(string.Format("<ul class=\"steps\" id=\"{0}\" style=\"display:none\">", scenario.Id), 10);

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
                AddLine(string.Format("<li class=\"step {0} {1} {2}\" onclick=\"toggle('{3}');\">",step.Result, stepClass, step.ExecutionOrder, step.Id), 11);
                AddLine(string.Format("<span>{0}</span>", result), 12);
                if (reportException)
                {
                    AddLine(string.Format("<div class=\"step {0}\" id=\"{1}\">",stepClass, step.Id), 12);
                    AddLine(string.Format("<code>{0}</code>", step.Exception.StackTrace), 13);
                    AddLine("</div>", 12);
                }
                AddLine("</li>", 11);
            }


            AddLine("</ul>", 10);
            AddLine("</div>", 8);       // End of scenario div
        }

        private void AddStoryMetaDataAndNarrative(Story story)
        {
            AddLine("<div class=\"storyMetaData\">", 7);

            if (story.MetaData == null)
            {
                var @namespace = story.Scenarios.First().TestObject.GetType().Namespace;
                AddLine(string.Format("div class=\"namespaceName\">{0}</div>", @namespace), 8);
            }
            else
            {
                AddLine(string.Format("<div class=\"storyTitle\">{0}</div>", story.MetaData.Title), 8);
            }

            if (story.MetaData != null && !string.IsNullOrEmpty(story.MetaData.AsA))
            {
                AddLine("<ul class=\"storyNarrative\">", 8);
                AddLine(string.Format("<li>{0}</li>", story.MetaData.AsA), 9);
                AddLine(string.Format("<li>{0}</li>", story.MetaData.IWant), 9);
                AddLine(string.Format("<li>{0}</li>", story.MetaData.SoThat), 9);
                AddLine("</ul>", 8);
            }


            AddLine("</div>", 7); // End of storyMetaData div
        }
    }
}
