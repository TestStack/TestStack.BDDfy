using NSubstitute;
using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.Readers;
using TestStack.BDDfy.Reporters.Writers;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class HtmlReporterTests
    {
        private readonly HtmlReporter _sut;
        private const string OutputPath = @"C:\Reports";
        private const string ReportData = "Report Data";
        private const string CustomStylesheet = "some custom css in here!";
        private const string CustomJavascript = "some custom javascript in here!";
        private const string ErrorMessage = "There was some exception.";

        public HtmlReporterTests()
        {
            _sut = new HtmlReporter(new HtmlReportConfiguration
            {
                FileWriter = Substitute.For<IFileWriter>(),
                FileReader = Substitute.For<IFileReader>(),
                ReportBuilder = Substitute.For<IReportBuilder>(),
                OutputPath = OutputPath
            });
        }

        [Fact]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            _sut.Configuration.ReportBuilder.CreateReport(Arg.Any<FileReportModel>()).Returns(ReportData);

            _sut.Process([]);

            _sut.Configuration.FileWriter.Received().WriteContents(ReportData, Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            _sut.Configuration.ReportBuilder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception(ErrorMessage); });

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Configuration.FileWriter.Received().WriteContents(
                Arg.Is<string>(s => s.StartsWith(ErrorMessage)),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Fact]
        public void ShouldLoadCustomStyleSheetIfOneExists()
        {
            var customStylesheetFilePath = Path.Combine(OutputPath, "BDDfyCustom.css");

            _sut.Configuration.FileReader.Exists(customStylesheetFilePath).Returns(true);
            _sut.Configuration.FileReader.Read(customStylesheetFilePath).Returns(CustomStylesheet);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomStylesheet.ShouldBe(CustomStylesheet);
            _sut.Configuration.FileReader.Received().Read(customStylesheetFilePath);
        }

        [Fact]
        public void ShouldNotLoadCustomStyleSheetIfNoneExist()
        {
            var customStylesheet = Path.Combine(OutputPath, "BDDfyCustom.css");

            _sut.Configuration.FileReader.Exists(customStylesheet).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomStylesheet.ShouldBe(null);
            _sut.Configuration.FileReader.DidNotReceive().Read(customStylesheet);
        }

        [Fact]
        public void ShouldLoadCustomJavascriptIfOneExists()
        {
            var javaScriptFilePath = Path.Combine(OutputPath, "BDDfyCustom.js");

            _sut.Configuration.FileReader.Exists(javaScriptFilePath).Returns(true);
            _sut.Configuration.FileReader.Read(javaScriptFilePath).Returns(CustomJavascript);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomJavascript.ShouldBe(CustomJavascript);
            _sut.Configuration.FileReader.Received().Read(javaScriptFilePath);
        }

        [Fact]
        public void ShouldNotLoadCustomJavascriptIfNoneExist()
        {
            var customJavascript = Path.Combine(OutputPath, "BDDfyCustom.js");

            _sut.Configuration.FileReader.Exists(customJavascript).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomJavascript.ShouldBe(null);
            _sut.Configuration.FileReader.DidNotReceive().Read(customJavascript);
        }
    }
}