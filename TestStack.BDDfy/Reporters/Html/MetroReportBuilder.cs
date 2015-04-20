using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters.Html
{
    public class MetroReportBuilder : IReportBuilder
    {
        private HtmlReportModel _model;
        readonly StringBuilder _html;
        const int TabIndentation = 2;
        int _tabCount;

        public MetroReportBuilder()
        {
            _html = new StringBuilder();
        }

        string IReportBuilder.CreateReport(FileReportModel model)
        {
            return CreateReport(model as HtmlReportModel);
        }

        public string CreateReport(HtmlReportModel model)
        {
            _model = model;
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
                EmbedCssFile(HtmlReportResources.metro_css_min);
                EmbedCssFile(_model.CustomStylesheet, HtmlReportResources.CustomStylesheetComment);
                AddLine("<link href='http://fonts.googleapis.com/css?family=Roboto:400,300' rel='stylesheet' type='text/css'>");
                AddLine(string.Format("<title>BDDfy Test Result {0}</title>", _model.RunDate.ToShortDateString()));
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
            using (OpenTag("<section id='titles'>", HtmlTag.section))
            {
                AddLine(string.Format("<h1 class='blue'>{0}</h1>", _model.Configuration.ReportHeader));
                AddLine(string.Format("<h3>{0}</h3>", _model.Configuration.ReportDescription));
            }
        }

        private void ResultSummary()
        {
            using (OpenTag("<section id='summaryTotals' class='group'>", HtmlTag.section))
            {
                using (OpenTag("<div class='tiles'>", HtmlTag.div))
                {
                    using (OpenTag("<div class='tilerow'>", HtmlTag.div))
                    {
                        using (OpenTag("<div id='storycount' class='tile tileNoHover two-h purplebg'>", HtmlTag.div))
                        {
                            AddLine("<h3>stories</h3>");
                            AddLine(string.Format("<h1>{0}</h1>", _model.Summary.Stories));
                        }
                        using (OpenTag("<div id='scenariocount' class='tile tileNoHover two-h tealbg'>", HtmlTag.div))
                        {
                            AddLine("<h3>scenarios</h3>");
                            AddLine(string.Format("<h1>{0}</h1>", _model.Summary.Scenarios));
                        }
                    }

                    using (OpenTag("<div class='tilerow' id='filterOptions'>", HtmlTag.div))
                    {

                        var tileOptions = new[]
                                          {
                                              Tuple.Create("Passed", "limebg", _model.Summary.Passed),
                                              Tuple.Create("Failed", "redbg", _model.Summary.Failed),
                                              Tuple.Create("Inconclusive", "orangebg", _model.Summary.Inconclusive),
                                              Tuple.Create("NotImplemented", "bluebg", _model.Summary.NotImplemented)
                                          };


                        foreach (var tileOption in tileOptions)
                        {
                            using (OpenTag("<a href='#'>", HtmlTag.a))
                            {
                                using (
                                    OpenTag(string.Format("<div class='tile one {0}' data-target-class='{1}'>", tileOption.Item2, tileOption.Item1), HtmlTag.div))
                                {
                                    AddLine(string.Format("<h4>{0}</h4>", tileOption.Item1.ToUpperInvariant()));
                                    AddLine(string.Format("<h1>{0}</h1>", tileOption.Item3));
                                }
                            }
                        }
                    }
                }
            }
        }



        private void ExpandCollapse()
        {
            using (OpenTag("<div id='expandCollapse' class='group'>", HtmlTag.div))
            {
                AddLine("<h2 style='float: left'>results</h2>");
                AddLine("<a href='#' class='expandAll'>show steps</a>");
                AddLine("<a href='#' class='collapseAll'>hide steps</a>");
            }
        }

        private void ResultDetails()
        {
            using (OpenTag("<section id='testResults'>", HtmlTag.section))
            {
                ExpandCollapse();

                using (OpenTag("<ul class='testResult'>", HtmlTag.ul))
                {
                    foreach (var story in _model.Stories)
                        AddStory(story);
                }
            }
        }

        private void Footer()
        {
            using (OpenTag(HtmlTag.section))
            {
                AddLine(string.Format("<p>Tested at: {0}</p>", _model.RunDate));
                AddLine("<p>Powered by <a href='https://github.com/TestStack/TestStack.BDDfy'>BDDfy</a></p>");
            }

            if (_model.Configuration.ResolveJqueryFromCdn)
                AddLine("<script type='text/javascript' src='http://code.jquery.com/jquery-2.1.0.min.js'></script>");
            else
                EmbedJavascriptFile(HtmlReportResources.jquery_2_1_0_min);

            EmbedJavascriptFile(HtmlReportResources.metro_js_min);
            EmbedJavascriptFile(_model.CustomJavascript, HtmlReportResources.CustomJavascriptComment);
        }

        private void AddStory(ReportModel.Story story)
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
                            AddScenario(scenario.ToArray());
                    }
                }
            }
        }

        private void AddScenario(ReportModel.Scenario[] scenarioGroup)
        {
            using (OpenTag(string.Format("<div class='scenario'>"), HtmlTag.div))
            {
                if (scenarioGroup.Count() == 1)
                    AddScenario(scenarioGroup.Single());
                else
                    AddScenarioWithExamples(scenarioGroup);
            }
        }

        private void AddScenarioWithExamples(ReportModel.Scenario[] scenarioGroup)
        {
            var firstScenario = scenarioGroup.First();
            var scenarioResult = (Result)scenarioGroup.Max(s => (int)s.Result);

            AddLine(string.Format("<div class='{0} canToggle scenarioTitle' data-toggle-target='{1}'>{2}{3}</div>", scenarioResult, firstScenario.Id, HttpUtility.HtmlEncode(firstScenario.Title), FormatTags(firstScenario.Tags)));

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
                            AddLine(string.Format("<div class='step-title-extra-lines'>{0}</div>", titleLines[i]));
                    }
                }

                AddExamples(scenarioGroup);
            }
        }

        private string FormatTags(List<string> tags)
        {
            return string.Join(string.Empty, tags.Select(t => string.Format("<div class='tag'>{0}</div>", t)));
        }

        private void AddExamples(ReportModel.Scenario[] scenarioGroup)
        {
            var firstScenario = scenarioGroup.First();
            var scenarioResult = (Result)scenarioGroup.Max(s => (int)s.Result);

            using (OpenTag("<li class='step'>", HtmlTag.li))
            {
                AddLine("<span class='example-header'>Examples:</span>");
                using (OpenTag(string.Format("<table class='examples' style='border-collapse: collapse;margin-left:10px''>"), HtmlTag.table))
                {
                    using (OpenTag("<tr>", HtmlTag.tr))
                    {
                        AddLine(string.Format("<th></th>"));
                        foreach (var header in firstScenario.Example.Headers)
                            AddLine(string.Format("<th>{0}</th>", header));

                        if (scenarioResult == Result.Failed)
                            AddLine(string.Format("<th>Error</th>"));
                    }

                    foreach (var scenario in scenarioGroup)
                        AddExampleRow(scenario, scenarioResult);
                }
            }
        }

        private void AddExampleRow(ReportModel.Scenario scenario, Result scenarioResult)
        {
            using (OpenTag("<tr>", HtmlTag.tr))
            {
                AddLine(string.Format("<td><Span class='{0}' style='margin-right:4px;' /></td>", scenario.Result));
                foreach (var exampleValue in scenario.Example.Values)
                    AddLine(string.Format("<td>{0}</td>", HttpUtility.HtmlEncode(exampleValue.GetValueAsString())));

                if (scenarioResult != Result.Failed)
                    return;

                using (OpenTag("<td>", HtmlTag.td))
                {
                    var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);

                    if (failingStep == null)
                        return;

                    var exceptionId = Configurator.IdGenerator.GetStepId();
                    var encodedExceptionMessage = HttpUtility.HtmlEncode(failingStep.Exception.Message);
                    AddLine(string.Format("<span class='canToggle' data-toggle-target='{0}'>{1}</span>", exceptionId, encodedExceptionMessage));
                    using (OpenTag(string.Format("<div class='step FailedException' id='{0}'>", exceptionId), HtmlTag.div))
                    {
                        AddLine(string.Format("<code>{0}</code>", failingStep.Exception.StackTrace));
                    }
                }
            }
        }

        private void AddScenario(ReportModel.Scenario scenario)
        {
            AddLine(string.Format("<div class='{0} canToggle scenarioTitle' data-toggle-target='{1}'>{2}{3}</div>", scenario.Result, scenario.Id, HttpUtility.HtmlEncode(scenario.Title), FormatTags(scenario.Tags)));

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
                                title += " [Exception Message: '" + step.Exception.Message + "']";
                        }

                        AddLine(string.Format("<span>{0}</span>", title));

                        for (int i = 1; i < titleLines.Length; i++)
                            AddLine(string.Format("<div class='step-title-extra-lines'>{0}</div>", titleLines[i]));

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

        private void AddStoryMetadataAndNarrative(ReportModel.Story story)
        {
            using (OpenTag("<div class='storyMetadata'>", HtmlTag.div))
            {
                AddLine(story.Metadata == null
                    ? string.Format("<h3 class='namespaceName'>{0}</h3>", story.Namespace)
                    : string.Format("<h3 class='storyTitle'>{0}{1}</h3>", story.Metadata.TitlePrefix, story.Metadata.Title));

                if (story.Metadata == null || string.IsNullOrEmpty(story.Metadata.Narrative1)) 
                    return;

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
    }
}
