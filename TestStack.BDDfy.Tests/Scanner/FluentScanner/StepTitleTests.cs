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
                .Then(_ => ThenTitleHas(AMethodCall()))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("Then title has Mutated state");
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