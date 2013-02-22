using System;
using System.Collections.Generic;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Processors.Diagnostics;
using TestStack.BDDfy.Processors.Reports;
using TestStack.BDDfy.Processors.Reports.MarkDown;

namespace TestStack.BDDfy.Processors
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implement IBatchProcessor and you are done
    /// </summary>
    public class MarkDownReporter : IBatchProcessor
    {
        private readonly IReportBuilder _builder;
        private readonly IReportWriter _writer;

        public MarkDownReporter() : this(new MarkDownReportBuilder(), new FileWriter()) { }

        public MarkDownReporter(IReportBuilder builder, IReportWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public void Process(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the markdown report: ";
            var viewModel = new FileReportModel(stories);
            string report;

            try
            {
                report = _builder.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            _writer.OutputReport(report, "BDDfy.md");
        }
    }
}
