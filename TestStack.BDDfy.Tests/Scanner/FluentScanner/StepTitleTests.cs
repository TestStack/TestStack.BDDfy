using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class StepTitleTests
    {
        private string _mutatedState;

        [Test]
        public void MethodCallInStepTitle()
        {
            FooClass something = new FooClass();
            var story = this
                .Given(_ => GivenWeMutateSomeState())
                .When(_ => something.Sub.SomethingHappens())
                .And(_ => something.Sub.SomethingWithDifferentTitle())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .And(_ => something.Sub.SomethingWithArg("foo"))
                .And(_ => something.Sub.SomethingWithArg2("foo"))
                .And(_ => something.Sub.SomethingWithArg3("foo"))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("And different title");
            story.Scenarios.Single().Steps.ElementAt(3).Title.ShouldBe("Then title has Mutated state");
            story.Scenarios.Single().Steps.ElementAt(4).Title.ShouldBe("And with arg foo");
            story.Scenarios.Single().Steps.ElementAt(5).Title.ShouldBe("And with arg");
            story.Scenarios.Single().Steps.ElementAt(6).Title.ShouldBe("And with foo arg");
        }

        public class FooClass
        {
            public FooClass()
            {
                Sub = new BarClass();
            }

            public BarClass Sub { get; set; }
        }

        public class BarClass
        {
            public void SomethingHappens()
            {
                
            }

            [StepTitle("Different title")]
            public void SomethingWithDifferentTitle()
            {
            }

            [StepTitle("With arg")]
            public void SomethingWithArg(string arg)
            {
            }

            [StepTitle("With arg", false)]
            public void SomethingWithArg2(string arg)
            {
            }

            [StepTitle("With {0} arg", false)]
            public void SomethingWithArg3(string arg)
            {
            }
        }

        private string AMethodCall()
        {
            return _mutatedState;
        }

        private void GivenWeMutateSomeState()
        {
            _mutatedState = "Mutated state";
        }

        private void ThenTitleHas(string result)
        {
            result.ShouldBe(_mutatedState);
        }
    }
}