using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.HtmlReporter
{
    public class HtmlReporter : IBatchProcessor
    {
        void WriteOutHtmlReport(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the html report: ";
            var htmlFullFileName = Path.Combine(_configuration.OutputPath, _configuration.OutputFileName);
            var viewModel = new HtmlReportViewModel(_configuration, stories);
            ShouldTheReportUseCustomization(viewModel);
            string report;

            try
            {
                report = new HtmlReportBuilder(viewModel).BuildReportHtml();
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            File.WriteAllText(htmlFullFileName, report);
        }

        private void ShouldTheReportUseCustomization(HtmlReportViewModel viewModel)
        {
            var customStylesheet = Path.Combine(_configuration.OutputPath, "bddifyCustom.css");
            viewModel.UseCustomStylesheet = File.Exists(customStylesheet);

            var customJavascript = Path.Combine(_configuration.OutputPath, "bddifyCustom.js");
            viewModel.UseCustomJavascript = File.Exists(customJavascript);
        }

        void WriteOutScriptFiles()
        {
            var cssFullFileName = Path.Combine(_configuration.OutputPath, "BDDfy.css");
            File.WriteAllText(cssFullFileName, HtmlReportResources.BDDfy_css);

            var jqueryFullFileName = Path.Combine(_configuration.OutputPath, "jquery-1.7.1.min.js");
            File.WriteAllText(jqueryFullFileName, HtmlReportResources.jquery_1_7_1_min);

            var bddifyJavascriptFileName = Path.Combine(_configuration.OutputPath, "BDDfy.js");
            File.WriteAllText(bddifyJavascriptFileName, HtmlReportResources.BDDfy_js);
        }

        readonly IHtmlReportConfiguration _configuration;

        public HtmlReporter(IHtmlReportConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Process(IEnumerable<Story> stories)
        {
            WriteOutScriptFiles();

            var allowedStories = stories.Where(s => _configuration.RunsOn(s)).ToList();
            WriteOutHtmlReport(allowedStories);
        }
    }
}