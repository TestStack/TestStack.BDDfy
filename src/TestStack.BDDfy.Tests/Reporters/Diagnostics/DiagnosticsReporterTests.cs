using NSubstitute;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.Writers;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    public class DiagnosticsReporterTests
    {
        private IReportBuilder _builder = null!;
        private IFileWriter _writer = null!;

        [Fact]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns("Report Data");

            sut.Process([]);

            _writer.Received().OutputReport("Report Data", Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            var sut = CreateSut();
            _builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception("Error occurred."); });

            sut.Process([]);

            _writer.Received().OutputReport(
                Arg.Is<string>(s => s.StartsWith("Error occurred.")),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        private GenericReporter<DiagnosticsReportBuilder> CreateSut()
        {
            _builder = Substitute.For<IReportBuilder>();
            _writer = Substitute.For<IFileWriter>();

            var configuration = new ReportConfiguration<DiagnosticsReportBuilder>("any")
            {
                ReportBuilder = _builder,
                FileWriter = _writer
            };

            return new GenericReporter<DiagnosticsReportBuilder>(configuration);
        }
    }
}