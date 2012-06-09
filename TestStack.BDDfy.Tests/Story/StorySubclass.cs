using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Story
{
    public class StoryAttributeIsInheritedFromBaseClass : StoryClassBase
    {
        Core.Story _story;

        [Test]
        public void Verify()
        {
            _story = this.BDDfy();
            Assert.That(_story.MetaData, Is.Not.Null);
            Assert.That(_story.MetaData.Title, Is.EqualTo(StoryTitle));
        }

        void WhenTheSubclassIsBddified()
        {
        }

        void ThenTheStoryAttributeIsFoundAndAssociatedWithTheSubStoryClass()
        {
        }
    }
}