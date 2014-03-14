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
                .When(_ => WhenStoryIsSpecifiedInFluentMode())
                .Then(_ => ThenTheSpecifiedStoryShouldBeUsed())
                .BDDfy<SharedStoryNotion>();

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.That(story.MetaData.Type, Is.EqualTo(typeof(SharedStoryNotion)));
        }

        void WhenStoryIsSpecifiedInFluentMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}