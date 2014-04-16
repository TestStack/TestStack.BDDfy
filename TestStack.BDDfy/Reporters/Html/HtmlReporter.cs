using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestStack.BDDfy.Reporters.Readers;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReporter : IBatchProcessor
    {
        public IReportBuilder ReportBuilder { get; set; }
        private readonly IReportWriter _writer;
        private readonly IFileReader _fileReader;
        readonly IHtmlReportConfiguration _configuration;
        public HtmlReportModel Model { get; private set; }

        public HtmlReporter(IHtmlReportConfiguration configuration)
            : this(configuration, new HtmlReportBuilder(), new FileWriter(), new FileReader())
        {
        }

        public HtmlReporter(IHtmlReportConfiguration configuration, IReportBuilder htmlReportBuilder)
            : this(configuration, htmlReportBuilder, new FileWriter(), new FileReader())
        {
        }

        public HtmlReporter(
            IHtmlReportConfiguration configuration, 
            IReportBuilder reportBuilder, 
            IReportWriter writer, 
            IFileReader reader)
        {
            _configuration = configuration;
            ReportBuilder = reportBuilder;
            _writer = writer;
            _fileReader = reader;
        }

        public void Process(IEnumerable<Story> stories)
        {
            var allowedStories = stories.Where(s => _configuration.RunsOn(s)).ToList();
            WriteOutHtmlReport(allowedStories);
        }

        void WriteOutHtmlReport(IEnumerable<Story> stories)
        {
            Model = new HtmlReportModel(_configuration, stories);
            LoadCustomScripts();
            string report;

            try
            {
                report = ReportBuilder.CreateReport(Model);
            }
            catch (Exception ex)
            {
                report = ex.Message + ex.StackTrace;
            }

            _writer.OutputReport(report, _configuration.OutputFileName, _configuration.OutputPath);
        }

        private void LoadCustomScripts()
        {
            var customStylesheet = Path.Combine(_configuration.OutputPath, "BDDfyCustom.css");
            if (_fileReader.Exists(customStylesheet))
                Model.CustomStylesheet = _fileReader.Read(customStylesheet);

            var customJavascript = Path.Combine(_configuration.OutputPath, "BDDfyCustom.js");
            if (_fileReader.Exists(customJavascript))
                Model.CustomJavascript = _fileReader.Read(customJavascript);
        }
    }
}