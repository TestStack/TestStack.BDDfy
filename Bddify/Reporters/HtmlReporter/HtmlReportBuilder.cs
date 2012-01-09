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

namespace Bddify.Reporters.HtmlReporter
{
    public class HtmlReportBuilder
    {
        readonly HtmlReportViewModel _viewModel;
        readonly StringBuilder _html;
        const int TabIndentation = 2;
        int _tabCount;

        public HtmlReportBuilder(HtmlReportViewModel viewModel)
        {
            _viewModel = viewModel;
            _html = new StringBuilder();
        }

        public string BuildReportHtml()
        {
            AddLine("<!DOCTYPE html>");
            using(OpenTag(HtmlTag.html))
            {
                HtmlHead();
                HtmlBody();
            }

            return _html.ToString();
        }

        private void HtmlHead()
        {
            using(OpenTag(HtmlTag.head))
            {
                AddLine("<meta charset='utf-8'/>");
                AddLine(string.Format("<link href='bddify.css' rel='stylesheet'/>"));
                AddLine("<script type='text/javascript' src='jquery-1.7.1.min.js'></script>");
                AddLine(string.Format("<script type='text/javascript' src='bddify.js'></script>"));
                AddLine(string.Format("<title>Bddify Test Result {0}</title>", DateTime.Now.ToShortDateString()));
            }
        }

        private void HtmlBody()
        {
            using(OpenTag(HtmlTag.body))
            {
                using(OpenTag("<div id='main'>", HtmlTag.div))
                {
                    Header();
                    ResultSummary();
                    ResultDetails();
                }

                Footer();
            }
        }

        private void Header()
        {
            using (OpenTag(HtmlTag.header))
            {
                AddLine(string.Format("<div id='bddifyTitle'>{0}</div>", _viewModel.Configuration.ReportHeader));
                AddLine(string.Format("<div id='bddifyDescription'>{0}</div>", _viewModel.Configuration.ReportDescription));
            }
        }

        private void ResultSummary()
        {
            using (OpenTag("<section class='summary'>", HtmlTag.section))
            {
                AddLine("<h3 class='label'>Summary:</h3>");

                using (OpenTag("<ul class='resultSummary'>", HtmlTag.ul))
                {
                    AddSummaryLine("story", "Stories", _viewModel.Summary.Stories);
                    AddSummaryLine("Passed", "Passed", _viewModel.Summary.Passed);
                    AddSummaryLine("Inconclusive", "Inconclusive", _viewModel.Summary.Inconclusive);
                    AddSummaryLine("Failed", "Failed", _viewModel.Summary.Failed);
                }
            }
        }

        private void AddSummaryLine(string cssClass, string label, int count)
        {
            using (OpenTag(string.Format("<li class='{0}'>", cssClass), HtmlTag.li))
            {
                using (OpenTag("<div class='summaryLine'>", HtmlTag.div))
                {
                    AddLine(string.Format("<div class='summaryLabel'>{0}</div>", label));
                    AddLine(string.Format("<span class='summaryCount'>{0}</span>", count));
                }
            }
        }

        private void ExpandCollapse()
        {
            using (OpenTag("<div id='expandCollapse'>", HtmlTag.div))
            {
                AddLine("<a href='#' class='expandAll'>[expand all]</a>");
                AddLine("<a href='#' class='collapseAll'>[collapse all]</a>");
            }
        }

        private void ResultDetails()
        {
            using (OpenTag("<div id='testResult'>", HtmlTag.div))
            {
                using (OpenTag("<div id='detailsHeader'>", HtmlTag.div))
                {
                    AddLine("<h3 id='detailsLable'>Details:</h3>");

                    ExpandCollapse();
                }

                using (OpenTag("<ul class='testResult'>", HtmlTag.ul))
                {
                    foreach (var story in _viewModel.Stories)
                    {
                        AddStory(story);
                    }
                }

                AddLine(string.Format("<p><span>Tested at: {0}</span></p>", DateTime.Now));
            }
        }

        private void Footer()
        {
            AddLine("<footer>Powered by <a href='https://code.google.com/p/bddify/'>bddify</a> framework</footer>");
        }

        private void AddStory(Story story)
        {
            var scenariosInGroup = story.Scenarios;
            var storyResult = (StepExecutionResult)scenariosInGroup.Max(s => (int)s.Result);

            using (OpenTag(HtmlTag.li))
            {
                using (OpenTag(string.Format("<div class='story {0}'>", storyResult), HtmlTag.div))
                {
                    AddStoryMetaDataAndNarrative(story);

                    using (OpenTag("<div class='scenarios'>", HtmlTag.div))
                    {
                        foreach (var scenario in scenariosInGroup)
                        {
                            AddScenario(scenario);
                        }
                    }
                }
            }
        }

        private void AddScenario(Scenario scenario)
        {
            using (OpenTag(string.Format("<div data-toggle-target='{0}' class='canToggle scenario {1}'>", scenario.Id, scenario.Result), HtmlTag.div))
            {
                AddLine(string.Format("<div class='scenarioTitle'>{0}</div>", scenario.Title));

                using (OpenTag(string.Format("<ul class='steps' id='{0}'>", scenario.Id), HtmlTag.ul))
                {
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

                        using (OpenTag(string.Format("<li class='canToggle step {0} {1} {2}' data-toggle-target='{3}' >", step.Result, stepClass, step.ExecutionOrder, step.Id), HtmlTag.li))
                        {
                            AddLine(string.Format("<span>{0}</span>", result));
                            if (reportException)
                            {
                                using (OpenTag(string.Format("<div class='step {0}' id='{1}'>", stepClass, step.Id), HtmlTag.div))
                                {
                                    AddLine(string.Format("<code>{0}</code>", step.Exception.StackTrace));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddStoryMetaDataAndNarrative(Story story)
        {
            using (OpenTag("<div class='storyMetaData'>", HtmlTag.div))
            {
                if (story.MetaData == null)
                {
                    var @namespace = story.Scenarios.First().TestObject.GetType().Namespace;
                    AddLine(string.Format("<div class='namespaceName'>{0}</div>", @namespace));
                }
                else
                {
                    AddLine(string.Format("<div class='storyTitle'>{0}</div>", story.MetaData.Title));
                }

                if (story.MetaData != null && !string.IsNullOrEmpty(story.MetaData.AsA))
                {
                    using (OpenTag("<ul class='storyNarrative'>", HtmlTag.ul))
                    {
                        AddLine(string.Format("<li>{0}</li>", story.MetaData.AsA));
                        AddLine(string.Format("<li>{0}</li>", story.MetaData.IWant));
                        AddLine(string.Format("<li>{0}</li>", story.MetaData.SoThat));
                    }
                }
            }
        }

        private HtmlReportTag OpenTag(string openingTag, HtmlTag tag)
        {
            AddLine(openingTag);
            _tabCount++;
            return new HtmlReportTag(tag, CloseTag);
        }

        private HtmlReportTag OpenTag(HtmlTag tag)
        {
            AddLine("<" + tag + ">");
            _tabCount++;
            return new HtmlReportTag(tag, CloseTag);
        }

        private void CloseTag(HtmlTag tag)
        {
            _tabCount--;
            var tagName = "</" + tag + ">";
            AddLine(tagName);
        }

        private void AddLine(string line)
        {
            int tabWidth = _tabCount * TabIndentation;
            _html.AppendLine(string.Empty.PadLeft(tabWidth) + line);
        }
    }
}
