using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Processors.Diagnostics;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    [TestFixture]
    public class DiagnosticsReporterSpecs
    {
        private ISerializer _serializer;
        private IReportWriter _writer;

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _serializer.Serialize(Arg.Any<object>()).Returns("Report Data");

            sut.Process(new List<Core.Story>());

            _writer.Received().Create("Report Data", Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _serializer.Serialize(Arg.Any<object>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process(new List<Core.Story>());

            _writer.Received().Create("There was an error compiling the json report: Error occurred.", Arg.Any<string>());
        }

        private DiagnosticsReporter CreateSut()
        {
            _serializer = Substitute.For<ISerializer>();
            _writer = Substitute.For<IReportWriter>();
            return new DiagnosticsReporter(_serializer, _writer);
        }
    }
}
