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
            var allowedStories = stories.Where(s => _configuration.RunsOn(s)).ToList();
            WriteOutHtmlReport(allowedStories);
        }

        void WriteOutHtmlReport(IEnumerable<Story> stories)
        {
            var viewModel = new HtmlReportViewModel(_configuration, stories);
            LoadCustomScripts(viewModel);
            string report;

            try
            {
                report = _builder.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                report = ex.Message + ex.StackTrace;
            }

            _writer.OutputReport(report, _configuration.OutputFileName, _configuration.OutputPath);
        }

        private void LoadCustomScripts(HtmlReportViewModel viewModel)
        {
            var customStylesheet = Path.Combine(_configuration.OutputPath, "BDDfyCustom.css");
            if (File.Exists(customStylesheet))
                viewModel.CustomStylesheet = File.ReadAllText(customStylesheet);

            var customJavascript = Path.Combine(_configuration.OutputPath, "BDDfyCustom.js");
            if(File.Exists(customJavascript))
                viewModel.CustomJavascript = File.ReadAllText(customJavascript);
        }
    }
}