using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    public class StoryAttributeIsInheritedFromBaseClass : StoryClassBase
    {
        Story _story;

        [Fact]
        public void Verify()
        {
            _story = this.BDDfy();
            _story.Metadata.ShouldNotBe(null);
            _story.Metadata.Title.ShouldBe(StoryTitle);
            _story.Metadata.TitlePrefix.ShouldBe(StoryTitlePrefix);
        }

        void WhenTheSubclassIsBddified()
        {
        }

        void ThenTheStoryAttributeIsFoundAndAssociatedWithTheSubStoryClass()
        {
        }
    }
}