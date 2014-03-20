using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    [TestFixture]
    [Story(
        AsA = "As a programmer",
        IWant = "the missing 'I want' to be added to story metadata",
        SoThat = "So that I don't have to duplicate it on the string")]
    public class WhenStoryAttibuteMissesIWantTextInIWantProperty
    {
        [Test]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenStoryAttibuteMissesIWantTextInIWantProperty>();

            Assert.That(story.Metadata.AsA, Is.EqualTo("As a programmer"));
            Assert.That(story.Metadata.IWant, Is.EqualTo("I want the missing 'I want' to be added to story metadata"));
            Assert.That(story.Metadata.SoThat, Is.EqualTo("So that I don't have to duplicate it on the string"));
        }
    }
}