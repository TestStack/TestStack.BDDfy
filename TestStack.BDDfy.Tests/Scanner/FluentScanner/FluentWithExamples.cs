using System.Runtime.CompilerServices;
using ApprovalTests;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class FluentWithExamples
    {
        [Test]
        public void FluentCanBeUsedWithExamples()
        {
            var story = this
                .Given(_ => GivenMethodTaking__ExampleInt__(Prop1), false)
                .And(_ => GivenMethodTaking__ExampleInt__(_.Prop1), false)
                .And(_ => GivenADifferentMethodWithRandomArg(2))
                .And(_ => GivenADifferentMethodWith(_prop2))
                .When(_ => WhenMethodUsing__ExampleString__())
                .When(_ => AndIUseA(multiWordHeading))
                .Then(_ => ThenAllIsGood())
                .WithExamples(new ExampleTable("Prop 1", "Prop2", "Prop 3", "Multi word heading")
                {
                    {1, "foo", ExecutionOrder.ConsecutiveAssertion, "" },
                    {2, "bar", ExecutionOrder.Initialize, "val2" }
                })
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            Approvals.Verify(textReporter.ToString());
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ExampleTypeMismatch()
        {
            var ex = Should.Throw<UnassignableExampleException>(
                () => this.Given(() => WrongType.ShouldBe(1), "Given i use an example")
                    .WithExamples(new ExampleTable("Wrong type")
                                      {
                                          new object(), 
                                          new object[] { null }
                                      })
                    .BDDfy());

            ex.Message.ShouldBe("Wrong type: System.Object cannot be assigned to Int32");
        }

        private void AndIUseA(string multiWordHeading)
        {
            multiWordHeading.ShouldBeOneOf("", "val2");
            this.multiWordHeading.ShouldBeOneOf("", "val2");
        }

        private void GivenADifferentMethodWith(string prop2)
        {
            _prop2.ShouldBeOneOf("foo", "bar");
        }

        private void GivenADifferentMethodWithRandomArg(int foo)
        {

        }

        private void ThenAllIsGood()
        {

        }

        private void WhenMethodUsing__ExampleString__()
        {
            _prop2.ShouldBeOneOf("foo", "bar");
            Prop_3.ShouldBeOneOf(ExecutionOrder.ConsecutiveAssertion, ExecutionOrder.Initialize);
        }

        private void GivenMethodTaking__ExampleInt__(int exampleInt)
        {
            exampleInt.ShouldBeInRange(1, 2);
        }

        public int WrongType { get; set; }
        public int Prop1 { get; set; }
        private string _prop2 = null;
        private string multiWordHeading = null;
        public ExecutionOrder Prop_3 { get; set; }
    }
}