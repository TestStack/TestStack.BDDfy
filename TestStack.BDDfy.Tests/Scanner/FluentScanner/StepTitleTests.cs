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
            var story = this
                .Given(_ => GivenWeMutateSomeState())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("Then title has Mutated state");
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