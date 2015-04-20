using System;
using System.Collections.Generic;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class DiagnosticsReporter : IBatchProcessor
    {
        private readonly IReportBuilder _builder;
        private readonly IReportWriter _writer;

        public DiagnosticsReporter() : this(new DiagnosticsReportBuilder(), new FileWriter()) { }

        public DiagnosticsReporter(IReportBuilder builder, IReportWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

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
