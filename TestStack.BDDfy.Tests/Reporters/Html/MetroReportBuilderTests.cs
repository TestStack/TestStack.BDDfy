using System;
using System.Runtime.CompilerServices;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class MetroReportBuilderTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            Func<FileReportModel> model = () => 
                new HtmlReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome())
                    {
                        RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                    };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtmlWithExamples()
        {
            Func<FileReportModel> model = () =>
                new HtmlReportModel(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples())
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}