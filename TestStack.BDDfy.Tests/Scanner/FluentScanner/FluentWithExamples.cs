using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class FluentWithExamples
    {
        [Test]
        public void FluentCanBeUsedWithExamples()
        {
            var story = this
                .Given(_ => GivenMethodTaking__ExampleInt__(Prop1), false)
                .And(_ => GivenMethodTaking__ExampleInt__(_.Prop1), false)
                .And(_ => GivenADifferentMethodWithRandomArg(2))
                .When(_ => WhenMethodUsing__ExampleString__())
                .Then(_ => ThenAllIsGood())
                .WithExamples(new ExampleTable("Prop1", "Prop2")
                {
                    {1, "foo"},
                    {2, "bar"}
                })
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            Approvals.Verify(textReporter.ToString());
        }

        private void GivenADifferentMethodWithRandomArg(int foo)
        {
            
        }

        private void ThenAllIsGood()
        {

        }

        private void WhenMethodUsing__ExampleString__()
        {
            Prop2.ShouldBeOneOf("foo", "bar");
        }

        private void GivenMethodTaking__ExampleInt__(int exampleInt)
        {
            exampleInt.ShouldBeInRange(1, 2);
        }

        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
    }
}