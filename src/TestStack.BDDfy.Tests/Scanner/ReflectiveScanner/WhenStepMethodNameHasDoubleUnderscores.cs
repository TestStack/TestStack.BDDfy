using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    /// <summary>
    /// Regression test for https://github.com/TestStack/TestStack.BDDfy/issues/201
    /// A double underscore in a step method name should not cause a stack overflow.
    /// </summary>
    public class WhenStepMethodNameHasDoubleUnderscores
    {
        private class StoryWithDoubleUnderscoreMethodNames
        {
            public bool GivenExecuted { get; private set; }
            public bool WhenExecuted { get; private set; }
            public bool ThenExecuted { get; private set; }

            public void Given_a__setup_step() => GivenExecuted = true;
            public void When_something__happens() => WhenExecuted = true;
            public void Then_the__result_is_correct() => ThenExecuted = true;
        }

        private class StoryWithPascalCaseDoubleUnderscore
        {
            public bool Executed { get; private set; }

            public void GivenSome__Precondition() => Executed = true;
            public void WhenAction__IsPerformed() { }
            public void ThenOutcome__IsExpected() { }
        }

        [Fact]
        public void DoesNotThrowForSnakeCaseWithDoubleUnderscore()
        {
            var story = new StoryWithDoubleUnderscoreMethodNames();

            var engine = story.LazyBDDfy();
            var exception = Record.Exception(() => engine.Run());

            exception.ShouldBeNull();
        }

        [Fact]
        public void AllStepsAreExecuted()
        {
            var story = new StoryWithDoubleUnderscoreMethodNames();
            story.BDDfy();

            story.GivenExecuted.ShouldBeTrue();
            story.WhenExecuted.ShouldBeTrue();
            story.ThenExecuted.ShouldBeTrue();
        }

        [Fact]
        public void StepTitlesAreGeneratedCorrectly_SnakeCase()
        {
            var story = new StoryWithDoubleUnderscoreMethodNames();
            var result = story.BDDfy();

            var steps = result.Scenarios.First().Steps.Select(s => s.Title).ToList();

            steps.ShouldContain("Given a setup step");
            steps.ShouldContain("When something happens");
            steps.ShouldContain("Then the result is correct");
        }

        [Fact]
        public void DoesNotThrowForPascalCaseWithDoubleUnderscore()
        {
            var story = new StoryWithPascalCaseDoubleUnderscore();

            var engine = story.LazyBDDfy();
            var exception = Record.Exception(() => engine.Run());

            exception.ShouldBeNull();
        }

        [Fact]
        public void StepTitlesAreGeneratedCorrectly_PascalCase()
        {
            var story = new StoryWithPascalCaseDoubleUnderscore();
            var result = story.BDDfy();

            var steps = result.Scenarios.First().Steps.Select(s => s.Title).ToList();

            steps.ShouldContain("Given some precondition");
            steps.ShouldContain("When action is performed");
            steps.ShouldContain("Then outcome is expected");
        }
    }
}
