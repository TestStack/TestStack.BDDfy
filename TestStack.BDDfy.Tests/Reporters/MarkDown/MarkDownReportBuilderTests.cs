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
            var model = new FileReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome().ToReportModel());
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdownWithExamples()
        {
            var model = 
                new FileReportModel(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                    .ToReportModel())
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}
