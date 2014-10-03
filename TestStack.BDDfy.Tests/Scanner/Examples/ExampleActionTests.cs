using ApprovalTests;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    [TestFixture]
    public class ExampleActionTests
    {
        private int value;

        [Test]
        public void CanUseActionsInExamples()
        {
            ExampleAction actionToPerform = null;
            int valueShouldBe = 0;
            var story = this.Given(_ => SomeSetup())
                .When(() => actionToPerform)
                .Then(_ => ShouldBe(valueShouldBe))
                .WithExamples(new ExampleTable("Action to perform", "Value should be")
                {
                    { new ExampleAction("Do something", () => { value = 42; }), 42 },
                    { new ExampleAction("Do something else", () => { value = 7; }), 7 }
                })
                .BDDfy();


            var textReporter = new TextReporter();
            textReporter.Process(story);
            Approvals.Verify(textReporter.ToString());
        }

        private void ShouldBe(int i)
        {
            value.ShouldBe(i);
        }

        private void SomeSetup()
        {
            
        }
    }
}