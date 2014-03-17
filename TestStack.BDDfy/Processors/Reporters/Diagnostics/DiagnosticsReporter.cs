﻿using System;
using System.Collections.Generic;

namespace TestStack.BDDfy.Processors
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
            const string error = "There was an error compiling the json report: ";
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

            _writer.OutputReport(report, "Diagnostics.json");
        }
    }
}
