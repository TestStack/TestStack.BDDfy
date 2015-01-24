using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story]
    public class StoryCanBeSpecifiedInReflectiveMode
    {
        [Fact] 
        public void Verify()
        {
            var story = this.BDDfy<SharedStoryNotion>();

            story.Metadata.ShouldBeAssignableTo<SharedStoryNotion>();
        }

        void WhenStoryIsSpecifiedInReflectiveMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}