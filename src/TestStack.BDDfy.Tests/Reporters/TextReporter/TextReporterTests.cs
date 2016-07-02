#if Approvals
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    public class TextReporterTests
    {
        [Fact]
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

            actual.ToString().ShouldMatchApproved(c => c.WithScrubber(ReportApprover.Scrub));
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedTextWithExamples()
        {
            var stories = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples();
            var actual = new StringBuilder();

            foreach (var story in stories)
            {
                var textReporter = new TextReporter();
                textReporter.Process(story);
                actual.AppendLine(textReporter.ToString());
            }

            actual.ToString().ShouldMatchApproved(c => c.WithScrubber(ReportApprover.Scrub));
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LongStepName()
        {
            var textReporter = new TextReporter();
            var scenario = new Scenario(typeof(TextReporterTests), new List<Step>
            {
                new Step(o => null, new StepTitle("Given a normal length title"), false, ExecutionOrder.SetupState, true, new List<StepArgument>()),
                new Step(o => null, new StepTitle("When something of normal length happens"), false, ExecutionOrder.Transition, true, new List<StepArgument>()),
                new Step(o => null, new StepTitle("Then some long state should be: #Title\r\n\r\nSome more stuff which is quite long on the second line\r\n\r\nAnd finally another really long line"),
                    true, ExecutionOrder.Assertion, true, new List<StepArgument>()),
                new Step(o => null, new StepTitle("And a normal length assertion"), true, ExecutionOrder.ConsecutiveAssertion, true, new List<StepArgument>())
            }, "Scenario Text", new List<string>());
            textReporter.Process(new Story(new StoryMetadata(typeof(TextReporterTests), new StoryNarrativeAttribute()),
                scenario));
            var actual = new StringBuilder();
            actual.AppendLine(textReporter.ToString());
            actual.ToString().ShouldMatchApproved(c => c.WithScrubber(ReportApprover.Scrub));
        }
    }
}
#endif