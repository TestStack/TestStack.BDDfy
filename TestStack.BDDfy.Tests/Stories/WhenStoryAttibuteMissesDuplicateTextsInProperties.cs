using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    [TestFixture]
    [Story(
        AsA = "programmer",
        IWant = "the missing texts to be added to story metadata",
        SoThat = "I don't have to duplicate it on the string")]
    public class WhenStoryAttibuteMissesDuplicateTextsInProperties
    {
        [Test]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenStoryAttibuteMissesDuplicateTextsInProperties>();

            Assert.That(story.Metadata.Narrative1, Is.EqualTo("As a programmer"));
            Assert.That(story.Metadata.Narrative2, Is.EqualTo("I want the missing texts to be added to story metadata"));
            Assert.That(story.Metadata.Narrative3, Is.EqualTo("So that I don't have to duplicate it on the string"));
        }
    }
}