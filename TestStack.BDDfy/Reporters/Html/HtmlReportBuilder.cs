using System;
using System.Linq;
using System.Text;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters.Html
{
    using System.Web;

    public class HtmlReportBuilder : IReportBuilder
    {
        private Func<DateTime> _dateProvider = () => DateTime.Now;
        private HtmlReportViewModel _viewModel;
        readonly StringBuilder _html;
        const int TabIndentation = 2;
        int _tabCount;

        public HtmlReportBuilder()
        {
            _html = new StringBuilder();
        }

        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportViewModel);
        }

        public string CreateReport(HtmlReportViewModel model)
        {
            _viewModel = model;
            AddLine("<!DOCTYPE html>");
            using (OpenTag(HtmlTag.html))
            {
                HtmlHead();
                HtmlBody();
            }

            return _html.ToString();
        }

        private void HtmlHead()
        {
            using (OpenTag(HtmlTag.head))
            {
                AddLine("<meta charset='utf-8'/>");
                EmbedCssFile(HtmlReportResources.BDDfy_css_min);
                EmbedCssFile(_viewModel.CustomStylesheet, HtmlReportResources.CustomStylesheetComment);

                AddLine(string.Format("<title>BDDfy Test Result {0}</title>", DateProvider().ToShortDateString()));
            }
        }

        private void HtmlBody()
        {
            using (OpenTag(HtmlTag.body))
            {
                using (OpenTag("<div id='main'>", HtmlTag.div))
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
            using (OpenTag("<div class='header'>", HtmlTag.div))
            {
                AddLine(string.Format("<div id='BDDfyTitle'>{0}</div>", _viewModel.Configuration.ReportHeader));
                AddLine(string.Format("<div id='BDDfyDescription'>{0}</div>", _viewModel.Configuration.ReportDescription));
            }
        }

        private void ResultSummary()
        {
            using (OpenTag("<section class='summary'>", HtmlTag.section))
            {
                AddLine("<h3 class='label'>Summary:</h3>");

                using (OpenTag("<ul class='resultSummary'>", HtmlTag.ul))
                {
                    AddSummaryLine("storySummary", "Stories", _viewModel.Summary.Stories);
                    AddSummaryLine("scenarioSummary", "Scenarios", _viewModel.Summary.Scenarios);
                    AddSummaryLine("Passed", "Passed", _viewModel.Summary.Passed);
                    AddSummaryLine("Inconclusive", "Inconclusive", _viewModel.Summary.Inconclusive);
                    AddSummaryLine("NotImplemented", "Not Implemented", _viewModel.Summary.NotImplemented);
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

                AddLine(string.Format("<p><span>Tested at: {0}</span></p>", _dateProvider()));
            }
        }

        private void Footer()
        {
            AddLine("<div class='footer'>Powered by <a href='https://github.com/TestStack/TestStack.BDDfy'>BDDfy</a> framework</div>");

            if (_viewModel.Configuration.ResolveJqueryFromCdn)
                AddLine("<script type='text/javascript' src='http://code.jquery.com/jquery-2.1.0.min.js'></script>");
            else
                EmbedJavascriptFile(HtmlReportResources.jquery_2_1_0_min);

            EmbedJavascriptFile(HtmlReportResources.BDDfy_js_min);
            EmbedJavascriptFile(_viewModel.CustomJavascript, HtmlReportResources.CustomJavascriptComment);
        }

        private void AddStory(Story story)
        {
            var scenariosInGroup = story.Scenarios.ToList();
            var scenariosGroupedById = story.Scenarios.GroupBy(s => s.Id);
            var storyResult = (Result)scenariosInGroup.Max(s => (int)s.Result);

            using (OpenTag(HtmlTag.li))
            {
                using (OpenTag(string.Format("<div class='story {0}'>", storyResult), HtmlTag.div))
                {
                    AddStoryMetadataAndNarrative(story);

                    using (OpenTag("<div class='scenarios'>", HtmlTag.div))
                    {
                        foreach (var scenario in scenariosGroupedById)
                        {
                            AddScenario(scenario.ToArray());
                        }
                    }
                }
            }
        }

        private void AddScenario(Scenario[] scenarioGroup)
        {
            using (OpenTag(string.Format("<div class='scenario'>"), HtmlTag.div))
            {
                if (scenarioGroup.Count() == 1)
                {
                    AddScenario(scenarioGroup.Single());
                }
                else
                {
                    AddScenarioWithExamples(scenarioGroup);
                }
            }
        }

        private void AddScenarioWithExamples(Scenario[] scenarioGroup)
        {
            var firstScenario = scenarioGroup.First();
            var scenarioResult = (Result)scenarioGroup.Max(s => (int)s.Result);

            AddLine(string.Format("<div class='{0} canToggle scenarioTitle' data-toggle-target='{1}'>{2}</div>", scenarioResult, firstScenario.Id, HttpUtility.HtmlEncode(firstScenario.Title)));

            using (OpenTag(string.Format("<ul class='steps' id='{0}'>", firstScenario.Id), HtmlTag.ul))
            {
                foreach (var step in firstScenario.Steps.Where(s => s.ShouldReport))
                {
                    using (OpenTag(string.Format("<li class='step {0}'>", step.ExecutionOrder), HtmlTag.li))
                    {
                        var titleLines = HttpUtility.HtmlEncode(step.Title)
                            .Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        var title = titleLines[0];

                        AddLine(string.Format("<span>{0}</span>", title));

                        for (int i = 1; i < titleLines.Length; i++)
                        {
                            AddLine(string.Format("<div class='step-title-extra-lines'>{0}</div>", titleLines[i]));
                        }
                    }
                }

                using (OpenTag("<li class='step'>", HtmlTag.li))
                {
                    AddLine("<h3>Examples:</h3>");
                    using (OpenTag(string.Format("<table class='examples' style='border-collapse: collapse;margin-left:10px''>"), HtmlTag.table))
                    {
                        using (OpenTag("<tr>", HtmlTag.tr))
                        {
                            AddLine(string.Format("<th></th>"));
                            foreach (var header in firstScenario.Example.Headers)
                            {
                                AddLine(string.Format("<th>{0}</th>", header));
                            }

                            if (scenarioResult == Result.Failed)
                            {
                                AddLine(string.Format("<th>Error</th>"));
                            }
                        }

                        foreach (var scenario in scenarioGroup)
                        {
                            using (OpenTag("<tr>", HtmlTag.tr))
                            {
                                AddLine(string.Format("<td><Span class='{0}' style='margin-right:4px;' /></td>", scenario.Result));
                                foreach (var value in scenario.Example.Values)
                                {
                                    AddLine(string.Format("<td>{0}</td>", HttpUtility.HtmlEncode(value)));
                                }

                                if (scenarioResult != Result.Failed)
                                    continue;

                                using (OpenTag("<td>", HtmlTag.td))
                                {
                                    var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);

                                    if (failingStep == null)
                                        continue;

                                    var exceptionId = Configurator.IdGenerator.GetStepId();
                                    AddLine(string.Format("<span class='canToggle' data-toggle-target='{0}'>{1}</span>", exceptionId, HttpUtility.HtmlEncode(failingStep.Exception.Message)));
                                    using (OpenTag(string.Format("<div class='step' id='{0}'>", exceptionId), HtmlTag.div))
                                    {
                                        AddLine(string.Format("<code>{0}</code>", failingStep.Exception.StackTrace));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddScenario(Scenario scenario)
        {
            AddLine(string.Format("<div class='{0} canToggle scenarioTitle' data-toggle-target='{1}'>{2}</div>", scenario.Result, scenario.Id, scenario.Title));

            using (OpenTag(string.Format("<ul class='steps' id='{0}'>", scenario.Id), HtmlTag.ul))
            {
                foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                {
                    string stepClass = string.Empty;
                    var reportException = step.Exception != null && step.Result == Result.Failed;
                    string canToggle = reportException ? "canToggle" : string.Empty;

                    using (OpenTag(string.Format("<li class='step {0} {1} {2} {3}' data-toggle-target='{4}' >", step.Result, stepClass, step.ExecutionOrder, canToggle, step.Id), HtmlTag.li))
                    {
                        var titleLines = step.Title.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        var title = titleLines[0];
                        if (reportException)
                        {
                            stepClass = step.Result + "Exception";
                            if (!string.IsNullOrEmpty(step.Exception.Message))
                            {
                                title += " [Exception Message: '" + step.Exception.Message + "']";
                            }
                        }

                        AddLine(string.Format("<span>{0}</span>", title));

                        for (int i = 1; i < titleLines.Length; i++)
                        {
                            AddLine(string.Format("<div class='step-title-extra-lines'>{0}</div>", titleLines[i]));
                        }

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

        private void AddStoryMetadataAndNarrative(Story story)
        {
            using (OpenTag("<div class='storyMetadata'>", HtmlTag.div))
            {
                if (story.Metadata == null)
                {
                    var @namespace = story.Namespace;
                    AddLine(string.Format("<div class='namespaceName'>{0}</div>", @namespace));
                }
                else
                {
                    AddLine(string.Format("<div class='storyTitle'>{0}</div>", story.Metadata.Title));
                }

                if (story.Metadata != null && !string.IsNullOrEmpty(story.Metadata.Narrative1))
                {
                    using (OpenTag("<ul class='storyNarrative'>", HtmlTag.ul))
                    {
                        AddLine(string.Format("<li>{0}</li>", story.Metadata.Narrative1));
                        if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                            AddLine(string.Format("<li>{0}</li>", story.Metadata.Narrative2));
                        if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                            AddLine(string.Format("<li>{0}</li>", story.Metadata.Narrative3));
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

        private void EmbedCssFile(string cssContent, string htmlComment = null)
        {
            using (OpenTag("<style type='text/css'>", HtmlTag.style))
            {
                AddHtmlComment(htmlComment);
                _html.AppendLine(cssContent);
            }
        }

        private void EmbedJavascriptFile(string javascriptContent, string htmlComment = null)
        {
            using (OpenTag(HtmlTag.script))
            {
                AddHtmlComment(htmlComment);
                _html.AppendLine(javascriptContent);
            }
        }

        private void AddHtmlComment(string htmlComment)
        {
            if (string.IsNullOrWhiteSpace(htmlComment))
                return;

            _html.AppendFormat("/*{0}*/", htmlComment);
            _html.AppendLine();
        }

        public Func<DateTime> DateProvider
        {
            get { return _dateProvider; }
            set { _dateProvider = value; }
        }
    }
}