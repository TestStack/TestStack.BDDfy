using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.Readers;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    public class HtmlMetroReporter : IBatchProcessor
    {
        private readonly IReportBuilder _builder;
        private readonly IReportWriter _writer;
        private readonly IFileReader _fileReader;
        readonly IHtmlReportConfiguration _configuration;
        public HtmlReportViewModel Model { get; private set; }

        public HtmlMetroReporter(IHtmlReportConfiguration configuration) 
            : this(configuration, new HtmlMetroReportBuilder(), new FileWriter(), new FileReader()) { }

        public HtmlMetroReporter(IHtmlReportConfiguration configuration, IReportBuilder builder, 
            IReportWriter writer, IFileReader reader)
        {
            _configuration = configuration;
            _builder = builder;
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
            Model = new HtmlReportViewModel(_configuration, stories);
            LoadCustomScripts();
            string report;

            try
            {
                report = _builder.CreateReport(Model);
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