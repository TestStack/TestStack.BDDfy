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
        private TestableHtmlReporter SUT;
        private const string OutputPath = @"C:\Reports";
        private const string ReportData = "Report Data";
        private const string ErrorMessage = "Error occurred.";

        [SetUp]
        public void SetUp()
        {
            SUT = TestableHtmlReporter.Create();
        }

        [Test]
        public void ShouldCreateReportIfProcessingSucceeds()
        {
            SUT.Builder.CreateReport(Arg.Any<FileReportModel>()).Returns(ReportData);

            SUT.Process(new List<Story>());

            SUT.Writer.Received().OutputReport(ReportData, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void ShouldPrintErrorInReportIfProcessingFails()
        {
            SUT.Builder.CreateReport(Arg.Any<FileReportModel>()).Returns(x => { throw new Exception(ErrorMessage); });

            SUT.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            SUT.Writer.Received().OutputReport(
                Arg.Is<string>(s => s.StartsWith(ErrorMessage)),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Test]
        public void ShouldLoadCustomStyleSheetIfOneExists()
        {
            const string customStylesheet = OutputPath + @"\BDDfyCustom.css";
            SUT.Configuration.OutputPath.Returns(OutputPath);
            SUT.FileReader.Exists(customStylesheet).Returns(true);
            SUT.FileReader.Read(customStylesheet).Returns(ReportData);

            SUT.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(SUT.Model.CustomStylesheet, Is.EqualTo(ReportData));
            SUT.FileReader.Received().Read(customStylesheet);
        }

        [Test]
        public void ShouldNotLoadCustomStyleSheetIfNoneExist()
        {
            const string customStylesheet = OutputPath + @"\BDDfyCustom.css";
            SUT.Configuration.OutputPath.Returns(OutputPath);
            SUT.FileReader.Exists(customStylesheet).Returns(false);

            SUT.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(SUT.Model.CustomStylesheet, Is.Null);
            SUT.FileReader.DidNotReceive().Read(customStylesheet);
        }

        [Test]
        public void ShouldLoadCustomJavascriptIfOneExists()
        {
            const string javaScript = OutputPath + @"\BDDfyCustom.js";
            SUT.Configuration.OutputPath.Returns(OutputPath);
            SUT.FileReader.Exists(javaScript).Returns(true);
            SUT.FileReader.Read(javaScript).Returns(ReportData);

            SUT.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(SUT.Model.CustomJavascript, Is.EqualTo(ReportData));
            SUT.FileReader.Received().Read(javaScript);
        }

        [Test]
        public void ShouldNotLoadCustomJavascriptIfNoneExist()
        {
            const string customJavascript = OutputPath + @"\BDDfyCustom.js";
            SUT.Configuration.OutputPath.Returns(OutputPath);
            SUT.FileReader.Exists(customJavascript).Returns(false);

            SUT.Process(new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            Assert.That(SUT.Model.CustomJavascript, Is.Null);
            SUT.FileReader.DidNotReceive().Read(customJavascript);
        }

    }
}
