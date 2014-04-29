using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story]
    public class StoryCanBeSpecifiedInFluentMode
    {
        [Test] 
        public void Verify()
        {
            var story = this
                .Given("Given step must be first")
                .When(_ => WhenStoryIsSpecifiedInFluentMode())
                .Then(_ => ThenTheSpecifiedStoryShouldBeUsed())
                .BDDfy<SharedStoryNotion>();

            Assert.That(story.Metadata, Is.Not.Null);
            Assert.That(story.Metadata.Type, Is.EqualTo(typeof(SharedStoryNotion)));
        }

        void WhenStoryIsSpecifiedInFluentMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}