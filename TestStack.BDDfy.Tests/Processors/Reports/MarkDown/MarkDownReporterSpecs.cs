using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Processors.Diagnostics;
using TestStack.BDDfy.Processors.Reports;

namespace TestStack.BDDfy.Tests.Processors.Reports.MarkDown
{
    [TestFixture]
    public class MarkDownReporterSpecs
    {
        private IReportBuilder _builder;
        private IReportWriter _writer;

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns("Report Data");

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("Report Data", Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("There was an error compiling the markdown report: Error occurred.", Arg.Any<string>());
        }

        private MarkDownReporter CreateSut()
        {
            _builder = Substitute.For<IReportBuilder>();
            _writer = Substitute.For<IReportWriter>();
            return new MarkDownReporter(_builder, _writer);
        }
    }
}
