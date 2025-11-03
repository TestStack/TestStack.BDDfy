using System;
using System.Runtime.CompilerServices;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [Trait("Category", TestCollectionName.Approvals)]
    public class ClassicReportBuilderTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            var model = 
                new HtmlReportModel(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds()
                    .ToReportModel())
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
            var reportData = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples().ToReportModel();
            var model = new HtmlReportModel(reportData)
            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new ClassicReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}