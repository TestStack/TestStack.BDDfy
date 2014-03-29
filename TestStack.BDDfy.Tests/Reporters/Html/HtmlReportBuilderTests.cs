using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    public class HtmlReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();

            var model = new HtmlReportViewModel(
                new DefaultHtmlReportConfiguration(),
                new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            var sut = new HtmlReportBuilder {DateProvider = () => new DateTime(2014, 3, 25, 11, 30, 5)};

            // setting the culture to make sure the date is formatted the same on all machines
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            
            var result = sut.CreateReport(model);
            Approvals.Verify(result);
        }
    }
}