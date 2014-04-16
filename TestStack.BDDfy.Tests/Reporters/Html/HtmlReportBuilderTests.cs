using System;
using System.Runtime.CompilerServices;
using ApprovalTests;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    public class HtmlReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();

            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var model = new HtmlReportViewModel(
                    new DefaultHtmlReportConfiguration(),
                    new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds());
                model.RunDate = new DateTime(2014, 3, 25, 11, 30, 5);

                var sut = new HtmlReportBuilder();
                var result = sut.CreateReport(model);
                Approvals.Verify(result, s => LineEndingsScrubber.Scrub(StackTraceScrubber.ScrubPaths(s)));
            }
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtmlWithExamples()
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();

            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var model = new HtmlReportViewModel(
                    new DefaultHtmlReportConfiguration(),
                    new ReportTestData()
                        .CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples())
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

                var sut = new HtmlReportBuilder();
                var result = sut.CreateReport(model);
                Approvals.Verify(result, s => LineEndingsScrubber.Scrub(StackTraceScrubber.ScrubPaths(s)));
            }
        }
    }
}