using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Processors.Reporters.Writers;

namespace TestStack.BDDfy.Processors.Reporters.Html
{
    public class HtmlReporter : IBatchProcessor
    {
        private readonly IReportBuilder _builder;
        private readonly IReportWriter _writer;
        readonly IHtmlReportConfiguration _configuration;

        public HtmlReporter(IHtmlReportConfiguration configuration) : this(configuration, new HtmlReportBuilder(), new FileWriter()) { }

        public HtmlReporter(IHtmlReportConfiguration configuration, IReportBuilder builder, IReportWriter writer)
        {
            _configuration = configuration;
            _builder = builder;
            _writer = writer;
        }

        public void Process(IEnumerable<Story> stories)
        {
            WriteOutScriptFiles();

            var allowedStories = stories.Where(s => _configuration.RunsOn(s)).ToList();
            WriteOutHtmlReport(allowedStories);
        }


        void WriteOutHtmlReport(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the html report: ";
            var viewModel = new HtmlReportViewModel(_configuration, stories);
            ShouldTheReportUseCustomization(viewModel);
            string report;

            try
            {
                report = _builder.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            _writer.OutputReport(report, _configuration.OutputFileName, _configuration.OutputPath);
        }

        private void ShouldTheReportUseCustomization(HtmlReportViewModel viewModel)
        {
            var customStylesheet = Path.Combine(_configuration.OutputPath, "BDDfyCustom.css");
            viewModel.UseCustomStylesheet = File.Exists(customStylesheet);

            var customJavascript = Path.Combine(_configuration.OutputPath, "BDDfyCustom.js");
            viewModel.UseCustomJavascript = File.Exists(customJavascript);
        }

        void WriteOutScriptFiles()
        {
            _writer.OutputReport(HtmlReportResources.BDDfy_css, "BDDfy.css", _configuration.OutputPath);
            _writer.OutputReport(HtmlReportResources.jquery_1_7_1_min, "jquery-1.7.1.min.js", _configuration.OutputPath);
            _writer.OutputReport(HtmlReportResources.BDDfy_js, "BDDfy.js", _configuration.OutputPath);
        }
    }
}