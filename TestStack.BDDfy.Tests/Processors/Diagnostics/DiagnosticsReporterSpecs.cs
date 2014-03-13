using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
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

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("Report Data", Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("There was an error compiling the json report: Error occurred.",
                                            Arg.Any<string>(), Arg.Any<string>());
        }

        private DiagnosticsReporter CreateSut()
        {
            _builder = Substitute.For<IReportBuilder>();
            _writer = Substitute.For<IReportWriter>();
            return new DiagnosticsReporter(_builder, _writer);
        }
    }
}
