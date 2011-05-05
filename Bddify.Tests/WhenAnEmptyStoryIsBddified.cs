using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests
{
    [Story(
        AsA = "As a programmer",
        IWant = "I want to first create an empty story",
        SoThat = "So that I can do test first development")]
    public class WhenAnEmptyStoryIsBddified
    {
        [Test]
        [IgnoreStep] // this is ignore here because this story is a scenario and having this as a step causes stackoverflow as bddify tries to invoke bddify in an endless loop
        public void ThenNoExeptionIsThrown()
        {
            this.Bddify();
        }
    }
}