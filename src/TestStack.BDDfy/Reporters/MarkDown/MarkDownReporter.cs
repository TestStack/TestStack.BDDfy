using System;
using System.Collections.Generic;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.MarkDown
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implement IBatchProcessor and you are done
    /// </summary>
    public class MarkDownReporter(IReportBuilder builder, IReportWriter writer): IBatchProcessor
    {
        private readonly IReportBuilder _builder = builder;
        private readonly IReportWriter _writer = writer;

        public MarkDownReporter() : this(new MarkDownReportBuilder(), new FileWriter()) { }

        public void Process(IEnumerable<Story> stories)
        {
            var viewModel = new FileReportModel(stories.ToReportModel());
            string report;

            try
            {
                report = _builder.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                report = ex.Message + ex.StackTrace;
            }

            _writer.OutputReport(report, "BDDfy.md");
        }
    }
}
