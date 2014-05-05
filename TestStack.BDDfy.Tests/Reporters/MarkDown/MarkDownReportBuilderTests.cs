using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.MarkDown;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    [TestFixture]
    public class MarkDownReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdown()
        {
            Func<FileReportModel> model = () => new FileReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome());
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdownWithExamples()
        {
            Func<FileReportModel> model = () =>
                new FileReportModel(new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples())
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}
