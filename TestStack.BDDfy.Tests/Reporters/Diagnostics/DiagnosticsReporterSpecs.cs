using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Writers;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    [TestFixture]
    public class DiagnosticsReporterSpecs
    {
        private IReportBuilder _builder;
        private IReportWriter _writer;

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns("Report Data");

            sut.Process(new List<Story>());

            _writer.Received().OutputReport("Report Data", Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
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
