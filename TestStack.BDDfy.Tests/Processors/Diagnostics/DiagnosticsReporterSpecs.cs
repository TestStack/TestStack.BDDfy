using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Processors.Diagnostics;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    [TestFixture]
    public class DiagnosticsReporterSpecs
    {
        private IDiagnosticsCalculator _calculator;
        private ISerializer _serializer;
        private IReportWriter _writer;

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _serializer.Serialize(Arg.Any<object>()).Returns("Report Data");

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("Report Data", Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _serializer.Serialize(Arg.Any<object>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process(new List<Core.Story>());

            _writer.Received().OutputReport("There was an error compiling the json report: Error occurred.", Arg.Any<string>());
        }

        [Test]
        public void ShouldGetDiagnosticDataFromStories()
        {
            var sut = CreateSut();
            sut.Process(new List<Core.Story>());
            _calculator.Received().GetDiagnosticData(Arg.Any<FileReportModel>());
        }

        [Test]
        public void ShouldSerializeDiagnosticDataToSpecifiedFormat()
        {
            var sut = CreateSut();
            sut.Process(new List<Core.Story>());
            _serializer.Received().Serialize(Arg.Any<IList<StoryDiagnostic>>());
        }

        private DiagnosticsReporter CreateSut()
        {
            _calculator = Substitute.For<IDiagnosticsCalculator>();
            _serializer = Substitute.For<ISerializer>();
            _writer = Substitute.For<IReportWriter>();
            return new DiagnosticsReporter(_calculator, _serializer, _writer);
        }
    }
}
