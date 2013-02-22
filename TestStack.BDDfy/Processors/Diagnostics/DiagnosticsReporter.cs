using System;
using System.Collections.Generic;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public class DiagnosticsReporter : IBatchProcessor
    {
        private readonly IDiagnosticsCalculator _calculator;
        private readonly ISerializer _serializer;
        private readonly IReportWriter _writer;

        public DiagnosticsReporter() : this(new DiagnosticsCalculator(),  new JsonSerializer(), new FileWriter()) { }

        public DiagnosticsReporter(IDiagnosticsCalculator calculator,  ISerializer serializer, IReportWriter writer)
        {
            _calculator = calculator;
            _serializer = serializer;
            _writer = writer;
        }

        public void Process(IEnumerable<Core.Story> stories)
        {
            const string error = "There was an error compiling the json report: ";
            var viewModel = new FileReportModel(stories);
            string report;

            try
            {
                var data = _calculator.GetDiagnosticData(viewModel);
                report = _serializer.Serialize(data);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            _writer.OutputReport(report, "Diagnostics.json");
        }
    }
}
