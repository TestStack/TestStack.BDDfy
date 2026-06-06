using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner
{
    public class FlattenArrayTests
    {
        [Fact]
        public void NestedStringArraysShouldDisplayValues()
        {
            var input = new object[] { new[] { "bob", "jim" }, new[] { "fred", "whoozit" } };

            var result = input.FlattenArray();

            result.ShouldBe("[bob, jim], [fred, whoozit]");
        }

        [Fact]
        public void FlatArrayShouldDisplayWithBrackets()
        {
            var input = new[] { "bob", "jim", "fred", "whoozit" };

            var result = input.FlattenArray();

            result.ShouldBe("bob, jim, fred, whoozit");
        }

        [Fact]
        public void MixedObjectArrayWithNestedArraysShouldDisplayCorrectly()
        {
            var input = new object[] { new[] { "bob", "jim" }, new object[] { 1, 2 }, "thingy", 3 };

            var result = input.FlattenArray();

            result.ShouldBe("[bob, jim], [1, 2], thingy, 3");
        }

        [Fact]
        public void NestedIntArraysShouldDisplayValues()
        {
            var input = new object[] { new[] { 1, 2 }, new[] { 3, 4 } };

            var result = input.FlattenArray();

            result.ShouldBe("[1, 2], [3, 4]");
        }

        [Fact]
        public void BDDfyScenarioWithNestedArrayArguments()
        {
            var names = new[] { "bob", "jim" };
            var numbers = new[] { 1, 2 };
            var mixed = new object[] { new[] { "alice", "eve" }, new[] { 3, 4 }, "scalar" };

            var story = this
                .Given(_ => GivenAnArrayOfNames(names))
                .And(_ => GivenAnArrayOfNumbers(numbers))
                .When(_ => WhenIPassAMixedNestedArray(mixed))
                .Then(_ => ThenTheStepTitlesDisplayArrayContents())
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            textReporter.ToString().ShouldMatchApproved();
        }

        private void GivenAnArrayOfNames(string[] names)
        {
            names.ShouldNotBeEmpty();
        }

        private void GivenAnArrayOfNumbers(int[] numbers)
        {
            numbers.ShouldNotBeEmpty();
        }

        private void WhenIPassAMixedNestedArray(object[] mixed)
        {
            mixed.Length.ShouldBeGreaterThan(0);
        }

        private void ThenTheStepTitlesDisplayArrayContents()
        {
        }
    }
}
