using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    public class HtmlReporterTests
    {
        private TestableHtmlReporter _sut;
        private const string OutputPath = @"C:\Reports";
        private const string ReportData = "Report Data";
        private const string CustomStylesheet = "some custom css in here!";
        private const string CustomJavascript = "some custom javascript in here!";
        private const string ErrorMessage = "There was some exception.";

        [SetUp]
        public void SetUp()
        {
            _sut = TestableHtmlReporter.Create();
        }

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            _sut.Builder.CreateReport(Arg.Any<FileReportModel>()).Returns(ReportData);

            _sut.Process(new List<Story>());

            _sut.Writer.Received().OutputReport(ReportData, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            _sut.Builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception(ErrorMessage); });

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            _sut.Writer.Received().OutputReport(
                Arg.Is<string>(s => s.StartsWith(ErrorMessage)),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Test]
        public void ShouldLoadCustomStyleSheetIfOneExists()
        {
            const string customStylesheetFilePath = OutputPath + @"\BDDfyCustom.css";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customStylesheetFilePath).Returns(true);
            _sut.FileReader.Read(customStylesheetFilePath).Returns(CustomStylesheet);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(_sut.Model.CustomStylesheet, Is.EqualTo(CustomStylesheet));
            _sut.FileReader.Received().Read(customStylesheetFilePath);
        }

        [Test]
        public void ShouldNotLoadCustomStyleSheetIfNoneExist()
        {
            const string customStylesheet = OutputPath + @"\BDDfyCustom.css";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customStylesheet).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(_sut.Model.CustomStylesheet, Is.Null);
            _sut.FileReader.DidNotReceive().Read(customStylesheet);
        }

        [Test]
        public void ShouldLoadCustomJavascriptIfOneExists()
        {
            const string javaScriptFilePath = OutputPath + @"\BDDfyCustom.js";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(javaScriptFilePath).Returns(true);
            _sut.FileReader.Read(javaScriptFilePath).Returns(CustomJavascript);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(_sut.Model.CustomJavascript, Is.EqualTo(CustomJavascript));
            _sut.FileReader.Received().Read(javaScriptFilePath);
        }

        [Test]
        public void ShouldNotLoadCustomJavascriptIfNoneExist()
        {
            const string customJavascript = OutputPath + @"\BDDfyCustom.js";
            _sut.Configuration.OutputPath.Returns(OutputPath);
            _sut.FileReader.Exists(customJavascript).Returns(false);

            _sut.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(_sut.Model.CustomJavascript, Is.Null);
            _sut.FileReader.DidNotReceive().Read(customJavascript);
        }
    }
}
