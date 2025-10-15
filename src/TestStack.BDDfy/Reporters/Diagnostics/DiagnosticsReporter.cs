using System;
using System.Collections.Generic;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class DiagnosticsReporter(IReportBuilder builder, IReportWriter writer): IBatchProcessor
    {
        private readonly IReportBuilder _builder = builder;
        private readonly IReportWriter _writer = writer;

        public DiagnosticsReporter() : this(new DiagnosticsReportBuilder(), new FileWriter()) { }

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

            _writer.OutputReport(report, "Diagnostics.json");
        }
    }
}
