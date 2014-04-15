using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    public class StoryAttributeIsInheritedFromBaseClass : StoryClassBase
    {
        Story _story;

        [Test]
        public void Verify()
        {
            _story = this.BDDfy();
            Assert.That(_story.Metadata, Is.Not.Null);
            Assert.That(_story.Metadata.Title, Is.EqualTo(StoryTitle));
            Assert.That(_story.Metadata.TitlePrefix, Is.EqualTo(StoryTitlePrefix));
        }

        void WhenTheSubclassIsBddified()
        {
        }

        void ThenTheStoryAttributeIsFoundAndAssociatedWithTheSubStoryClass()
        {
        }
    }
}