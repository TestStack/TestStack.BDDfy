using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    [TestFixture]
    [Story(
        InOrderTo = "do something",
        AsA = "programmer",
        IWant = "this to work")]
    public class CanUseInOrderToSyntax
    {
        [Test]
        public void When_InOrderTo_is_specified_the_InOrderTo_syntax_is_used()
        {
            var story = new DummyScenario().BDDfy<CanUseInOrderToSyntax>();

            Assert.That(story.Metadata.Narrative1, Is.EqualTo("In order to do something"));
            Assert.That(story.Metadata.Narrative2, Is.EqualTo("As a programmer"));
            Assert.That(story.Metadata.Narrative3, Is.EqualTo("I want this to work"));
        }
    }
}
