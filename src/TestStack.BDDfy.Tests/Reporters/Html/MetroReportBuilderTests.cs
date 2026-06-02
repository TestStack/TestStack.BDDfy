using SampleStories;
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
            var model = 
                new HtmlReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome().ToReportModel())
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
            var model = 
                new HtmlReportModel(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
                    .ToReportModel())
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtmlWithExternalStories()
        {
            var story = new LameExternalStory();
            var bddfiedStories = new[] { story.BDDfy() };

            var reportModel = bddfiedStories.ToReportModel();
            var model = new HtmlReportModel(reportModel)
            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}