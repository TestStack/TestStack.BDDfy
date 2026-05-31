using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    public class StoryAttributeIsInheritedFromBaseClass : StoryClassBase
    {
        [Fact]
        public void Verify()
        {
            var story = this.BDDfy();
            story.Metadata.ShouldNotBeNull();
            story.Metadata.Title.ShouldBe(StoryTitle);
            story.Metadata.TitlePrefix.ShouldBe(StoryTitlePrefix);
        }

        void WhenTheSubclassIsBddified()
        {
        }

        void ThenTheStoryAttributeIsFoundAndAssociatedWithTheSubStoryClass()
        {
        }
    }
}