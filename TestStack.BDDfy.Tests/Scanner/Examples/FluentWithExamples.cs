using System.Runtime.CompilerServices;
using ApprovalTests;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    [TestFixture]
    public class FluentWithExamples
    {
        [Test]
        public void FluentCanBeUsedWithExamples()
        {
            var story = this
                .Given(_ => MethodTaking__ExampleInt__(Prop1), false)
                .And(_ => MethodTaking__ExampleInt__(_.Prop1), false)
                .And(_ => ADifferentMethodWithRandomArg(2))
                .And(_ => ADifferentMethodWith(_prop2))
                .When(_ => WhenMethodUsing__ExampleString__())
                .And(_ => AndIUseA(multiWordHeading))
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

        private void GivenIntWithValue(int differentName)
        {
            differentName.ShouldBeOneOf(1, 2);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Inline()
        {
            // ReSharper disable once ConvertToConstant.Local
            var inlineVariable = 0;
            var story = this
                .Given(_ => GivenIntWithValue(inlineVariable))
                .WithExamples(new ExampleTable("Inline Variable") { 1, 2 })
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

            ex.Message.ShouldBe("System.Object cannot be assigned to Int32 (Column: 'Wrong type', Row: 1)");
        }

        private void AndIUseA(string multiWordHeading)
        {
            multiWordHeading.ShouldBeOneOf("", "val2");
            this.multiWordHeading.ShouldBeOneOf("", "val2");
        }

        private void ADifferentMethodWith(string prop2)
        {
            _prop2.ShouldBeOneOf("foo", "bar");
        }

        private void ADifferentMethodWithRandomArg(int foo)
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

        private void MethodTaking__ExampleInt__(int exampleInt)
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