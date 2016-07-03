using System;
using System.Collections.Generic;
using NSubstitute;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Writers;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    public class DiagnosticsReporterTests
    {
        private IReportBuilder _builder;
        private IReportWriter _writer;

        [Fact]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns("Report Data");

            sut.Process(new List<Story>());

            _writer.Received().OutputReport("Report Data", Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process(new List<Story>());

            _writer.Received().OutputReport(
                Arg.Is<string>(s => s.StartsWith("Error occurred.")),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        private DiagnosticsReporter CreateSut()
        {
            _builder = Substitute.For<IReportBuilder>();
            _writer = Substitute.For<IReportWriter>();
            return new DiagnosticsReporter(_builder, _writer);
        }
    }
}