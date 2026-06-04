using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    /// <summary>
    /// Test for https://github.com/TestStack/TestStack.BDDfy/issues/165
    /// Steps that return a string or IEnumerable&lt;string&gt; in the fluent API
    /// should use the returned value as the step title, allowing dynamic titles
    /// without specifying a title parameter on every call.
    /// </summary>
    public class FluentStepReturningDynamicTitle
    {
        private int _input;

        string GivenWithStringReturn()
        {
            _input = 42;
            return "Given the system is initialised with value 42";
        }

        string WhenWithStringReturn()
        {
            _input *= 2;
            return "When the value is doubled";
        }

        string ThenWithStringReturn()
        {
            _input.ShouldBe(84);
            return "Then the value is 84";
        }

        IEnumerable<string> GivenWithEnumerableReturn()
        {
            yield return "Given setup via enumerable";
            _input = 10;
        }

        IEnumerable<string> WhenWithEnumerableReturn()
        {
            yield return "When action via enumerable";
            _input += 5;
        }

        IEnumerable<string> ThenWithEnumerableReturn()
        {
            yield return "Then result via enumerable";
            _input.ShouldBe(15);
        }

        string WhenThrowingString() => throw new InvalidOperationException("string step failed");

        IEnumerable<string> WhenThrowingEnumerable()
        {
            throw new InvalidOperationException("enumerable step failed");
        }

        IEnumerable<string> WhenYieldsThenThrows()
        {
            yield return "When it yields then throws an exception";
            throw new InvalidOperationException("failed after yield");
        }

        [Fact]
        public void ShouldUseDynamicTitleFromStringReturnValue()
        {
            var story = this
                .Given(_ => _.GivenWithStringReturn())
                .When(_ => _.WhenWithStringReturn())
                .Then(_ => _.ThenWithStringReturn())
                .BDDfy();

            var scenario = story.Scenarios.First();
            var titles = scenario.Steps.Select(s => s.Title).ToArray();

            titles.ShouldBe([
                "Given the system is initialised with value 42",
                "When the value is doubled",
                "Then the value is 84"
            ]);
        }

        [Fact]
        public void ShouldUseDynamicTitleFromEnumerableReturnValue()
        {
            var story = this
                .Given(_ => _.GivenWithEnumerableReturn())
                .When(_ => _.WhenWithEnumerableReturn())
                .Then(_ => _.ThenWithEnumerableReturn())
                .BDDfy();

            var scenario = story.Scenarios.First();
            var titles = scenario.Steps.Select(s => s.Title).ToArray();

            titles.ShouldBe([
                "Given setup via enumerable",
                "When action via enumerable",
                "Then result via enumerable"
            ]);
        }

        [Fact]
        public void ShouldThrowWhenStringReturningStepThrows()
        {
            Should.Throw<InvalidOperationException>(() =>
                this.Given(_ => _.GivenWithStringReturn())
                    .When(_ => _.WhenThrowingString())
                    .Then(_ => _.ThenWithStringReturn())
                    .BDDfy());
        }

        [Fact]
        public void ShouldThrowWhenEnumerableReturningStepThrows()
        {
            Should.Throw<InvalidOperationException>(() =>
                this.Given(_ => _.GivenWithEnumerableReturn())
                    .When(_ => _.WhenThrowingEnumerable())
                    .Then(_ => _.ThenWithEnumerableReturn())
                    .BDDfy());
        }

        [Fact]
        public void ShouldSetTitleBeforeThrowingWhenEnumerableYieldsThenThrows()
        {
            // The step yields a title then throws — the title should be set
            // and the exception should still propagate as a step failure.
            var ex = Should.Throw<InvalidOperationException>(() =>
                this.Given(_ => _.GivenWithEnumerableReturn())
                    .When(_ => _.WhenYieldsThenThrows())
                    .Then(_ => _.ThenWithEnumerableReturn())
                    .BDDfy());

            ex.Message.ShouldBe("failed after yield");
        }
    }
}
