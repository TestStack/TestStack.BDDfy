using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
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
            var model = new HtmlReportViewModel(
                new DefaultHtmlReportConfiguration(),
                new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());
            var sut = new HtmlReportBuilder {DateProvider = () => new DateTime(2014, 3, 25, 11, 30, 5)};

            var result = sut.CreateReport(model);

            Approvals.Verify(result);
        }
    }
}