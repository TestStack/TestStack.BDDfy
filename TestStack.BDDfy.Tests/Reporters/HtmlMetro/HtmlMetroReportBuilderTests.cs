using System;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.HtmlMetro;
using TestStack.BDDfy.Tests.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.HtmlMetro
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    public class HtmlMetroReportBuilderTests
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
                    new ReportTestData().CreateMixContainingEachTypeOfOutcome());

                var sut = new HtmlMetroReportBuilder {DateProvider = () => new DateTime(2014, 3, 25, 11, 30, 5)};
                var result = sut.CreateReport(model);
                Approvals.Verify(result, s => LineEndingsScrubber.Scrub(StackTraceScrubber.ScrubPaths(s)));
            }
        }
    }
}