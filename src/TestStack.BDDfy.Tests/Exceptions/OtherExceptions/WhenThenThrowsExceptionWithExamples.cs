using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenThenThrowsExceptionWithExamples
    {
        private InvalidOperationException _stepException = new("Ooops!");
        private void StepThrowsException(int input) => throw _stepException;

        [Fact]
        public void EachFailedAssertionExampleRunsTeardown()
        {
            var teardowns = 0;
            var input = 0;
            var engine = this.Given("i have a test with examples")
                .When("i run that test")
                .Then(_=> StepThrowsException(input))
                .WithExamples(new ExampleTable("input")
                {
                    { 1 }, { 2 }, { 3 }
                }).TearDownWith(() => teardowns++ , "teardown")
                .LazyBDDfy();

            Should.Throw<InvalidOperationException>(engine.Run);           
            teardowns.ShouldBe(3);

            engine.Story.Result.ShouldBe(Result.Failed);
            var titles = engine.Story.Scenarios.Select(s => s.Steps.Single(step => step.Result == Result.Failed).Title).ToArray();
            titles.ShouldAllBe(t => t.StartsWith("Then", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EachFailedGivenExampleRunsTeardown()
        {
            var teardowns = 0;
            var input = 0;
            var engine = this.Given(_=> StepThrowsException(input))
                .When("i run that test")
                .Then("will never happen")
                .WithExamples(new ExampleTable("input")
                {
                    { 1 }, { 2 }, { 3 }
                }).TearDownWith(() => teardowns++, "teardown")
                .LazyBDDfy();

            Should.Throw<InvalidOperationException>(engine.Run);
            teardowns.ShouldBe(3);

            engine.Story.Result.ShouldBe(Result.Failed);
            var titles = engine.Story.Scenarios.Select(s=>s.Steps.Single(step=>step.Result == Result.Failed).Title).ToArray();
            titles.ShouldAllBe(t=> t.StartsWith("Given", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EachFailedWhenExampleRunsTeardown()
        {
            var teardowns = 0;
            var input = 0;
            var engine = this.Given("i have a test with examples")
                .When(_ => StepThrowsException(input))
                .Then("will never happen")
                .WithExamples(new ExampleTable("input")
                {
                    { 1 }, { 2 }, { 3 }
                }).TearDownWith(() => teardowns++, "teardown")
                .LazyBDDfy();

            Should.Throw<InvalidOperationException>(engine.Run);
            teardowns.ShouldBe(3);

            engine.Story.Result.ShouldBe(Result.Failed);
            var titles = engine.Story.Scenarios.Select(s => s.Steps.Single(step => step.Result == Result.Failed).Title).ToArray();
            titles.ShouldAllBe(t => t.StartsWith("When", StringComparison.OrdinalIgnoreCase));
        }
    }
}
