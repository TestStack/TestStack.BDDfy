using System;
using System.Runtime.CompilerServices;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class ClassicReportBuilderTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            var reportModel = new ReportTestData()
                .CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds()
                .ToReportModel();

            var model = new HtmlReportModel(reportModel)
                    {
                        RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                    };

            var sut = new ClassicReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtmlWithExamples()
        {
            var reportModel = new ReportTestData()
                .CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                .ToReportModel();

            var model = new HtmlReportModel(reportModel)
            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new ClassicReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}