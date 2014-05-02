using System;
using System.Runtime.CompilerServices;
using System.Text;
using ApprovalTests;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    [TestFixture]
    public class TextReporterTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedReport()
        {
            var stories = new ReportTestData().CreateMixContainingEachTypeOfOutcomeWithOneScenarioPerStory();
            var actual = new StringBuilder();

            foreach (var story in stories)
            {
                var textReporter = new TextReporter();
                textReporter.Process(story);
                actual.AppendLine(textReporter.ToString());
            }

            Approvals.Verify(actual.ToString(), StackTraceScrubber.Scrub);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdownWithExamples()
        {
            var stories = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples();
            var actual = new StringBuilder();

            foreach (var story in stories)
            {
                var textReporter = new TextReporter();
                textReporter.Process(story);
                actual.AppendLine(textReporter.ToString());
            }

            Approvals.Verify(actual.ToString(), StackTraceScrubber.Scrub);
        }
    }
}
