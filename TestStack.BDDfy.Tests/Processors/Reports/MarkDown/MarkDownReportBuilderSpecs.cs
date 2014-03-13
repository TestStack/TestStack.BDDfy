using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;

namespace TestStack.BDDfy.Tests.Processors.Reports.MarkDown
{
    [TestFixture]
    public class MarkDownReportBuilderSpecs
    {
        private MarkDownReportBuilder _sut;
        private IEnumerable<Core.Story> _stories;
        private string[] _result;

        [Given("Given a MarkDownReportBuilder")]
        public void GivenAMarkDownReportBuilder()
        {
            _sut = new MarkDownReportBuilder();
        }

        public void AndGivenTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds()
        {
            _stories = new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds();
        }

        public void WhenTheReportIsCreated()
        {
            var report = _sut.CreateReport(new FileReportModel(_stories));
            _result = Regex.Split(report, "\r\n");
        }

        public void ThenTheFirstLineShouldBeTheFirstStoryTitleAsH2()
        {
            AssertLine(0, "## Story: Account holder withdraws cash");
        }

        public void AndThenShouldDisplayStoryIndentedByOneSpaceWithBoldFormatting()
        {
            AssertLine(1, " **As an account holder**  ");
            AssertLine(2, " **I want to withdraw cash**  ");
            AssertLine(3, " **So that I can get money when the bank is closed**  ");
        }

        public void AndThenShouldDisplayALineBreakAfterTheStory()
        {
            AssertLine(4, string.Empty);
        }

        public void AndThenShouldDisplayScenarioTitleAsH3()
        {
            AssertLine(5, "### Happy Path Scenario");
        }

        public void AndThenShouldDisplayScenarioStepsIndentedByTwoSpaces()
        {
            AssertLine(6, "  Given a positive account balance  ");
            AssertLine(7, "  When the account holder requests money  ");
            AssertLine(8, "  Then money is dispensed  ");
        }

        public void AndThenShouldDisplayALineBreakBetweenScenarios()
        {
            AssertLine(9, string.Empty);
            AssertLine(10, "### Sad Path Scenario");
        }

        public void AndThenShouldRepeatForSubsequentStories()
        {
            AssertLine(14, string.Empty);
            AssertLine(15, "## Story: Happiness");
        }

        private void AssertLine(int lineNumber, string lineValue)
        {
            Assert.That(_result[lineNumber], Is.EqualTo(lineValue));
        }

        [Test]
        public void RunSpecs()
        {
            this.BDDfy();
        }
    }
}
