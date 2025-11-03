using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    [Trait("Category", TestCollectionName.Approvals)]
    public class ExampleActionTests
    {
        private int _value;

        [Fact]
        public void CanUseActionsInExamples()
        {
            ExampleAction actionToPerform = null;
            int valueShouldBe = 0;
            var story = this.Given(_ => SomeSetup())
                .When(() => actionToPerform)
                .Then(_ => ShouldBe(valueShouldBe))
                .WithExamples(new ExampleTable("Action to perform", "Value should be")
                {
                    { new ExampleAction("Do something", () => { _value = 42; }), 42 },
                    { new ExampleAction("Do something else", () => { _value = 7; }), 7 }
                })
                .BDDfy();


            var textReporter = new TextReporter();
            textReporter.Process(story);
            textReporter.ToString().ShouldMatchApproved();
        }

        private void ShouldBe(int i)
        {
            _value.ShouldBe(i);
        }

        private void SomeSetup()
        {
            
        }
    }
}