using System;
using System.Collections.Generic;
using NSubstitute;
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [Collection("ExclusiveAccessToConfigurator")]
    public class HtmlReporterTests
    {
        private TestableHtmlReporter _sut;
        private const string OutputPath = @"C:\Reports";
        private const string ReportData = "Report Data";
        private const string CustomStylesheet = "some custom css in here!";
        private const string CustomJavascript = "some custom javascript in here!";
        private const string ErrorMessage = "There was some exception.";

        public HtmlReporterTests()
        {
            _sut = TestableHtmlReporter.Create();
        }

        [Fact]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            _sut.ReportBuilder.CreateReport(Arg.Any<FileReportModel>()).Returns(ReportData);

            _sut.Process(new List<Story>());

            _sut.Writer.Received().OutputReport(ReportData, Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            _sut.ReportBuilder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception(ErrorMessage); });

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Writer.Received().OutputReport(
                Arg.Is<string>(s => s.StartsWith(ErrorMessage)),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Fact]
        public void ShouldLoadCustomStyleSheetIfOneExists()
        {
            const string customStylesheetFilePath = OutputPath + @"\BDDfyCustom.css";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customStylesheetFilePath).Returns(true);
            _sut.FileReader.Read(customStylesheetFilePath).Returns(CustomStylesheet);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomStylesheet.ShouldBe(CustomStylesheet);
            _sut.FileReader.Received().Read(customStylesheetFilePath);
        }

        [Fact]
        public void ShouldNotLoadCustomStyleSheetIfNoneExist()
        {
            const string customStylesheet = OutputPath + @"\BDDfyCustom.css";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customStylesheet).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomStylesheet.ShouldBe(null);
            _sut.FileReader.DidNotReceive().Read(customStylesheet);
        }

        [Fact]
        public void ShouldLoadCustomJavascriptIfOneExists()
        {
            const string javaScriptFilePath = OutputPath + @"\BDDfyCustom.js";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(javaScriptFilePath).Returns(true);
            _sut.FileReader.Read(javaScriptFilePath).Returns(CustomJavascript);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomJavascript.ShouldBe(CustomJavascript);
            _sut.FileReader.Received().Read(javaScriptFilePath);
        }

        [Fact]
        public void ShouldNotLoadCustomJavascriptIfNoneExist()
        {
            const string customJavascript = OutputPath + @"\BDDfyCustom.js";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customJavascript).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());

            _sut.Model.CustomJavascript.ShouldBe(null);
            _sut.FileReader.DidNotReceive().Read(customJavascript);
        }
    }
}