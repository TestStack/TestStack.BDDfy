using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story]
    public class StoryCanBeSpecifiedInFluentMode
    {
        [Fact] 
        public void Verify()
        {
            var story = this
                .When(_ => WhenStoryIsSpecifiedInFluentMode())
                .Then(_ => ThenTheSpecifiedStoryShouldBeUsed())
                .BDDfy<SharedStoryNotion>();

            story.Metadata.ShouldBeAssignableTo<SharedStoryNotion>();
        }

        void WhenStoryIsSpecifiedInFluentMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}