using System;
using System.Runtime.CompilerServices;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.MarkDown;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    public class MarkDownReportBuilderTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdown()
        {
            var reportModel = new ReportTestData().CreateMixContainingEachTypeOfOutcome().ToReportModel();
            var model = new FileReportModel(reportModel);
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdownWithExamples()
        {
            var reportModel = new ReportTestData()
                .CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                .ToReportModel();

            var model =  new FileReportModel(reportModel)
            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}