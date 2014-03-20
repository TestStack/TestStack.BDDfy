using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story]
    public class StoryCanBeSpecifiedInReflectiveMode
    {
        [Test] 
        public void Verify()
        {
            var story = this.BDDfy<SharedStoryNotion>();

            Assert.That(story.Metadata, Is.Not.Null);
            Assert.AreEqual(story.Metadata.Type, typeof(SharedStoryNotion));
        }

        void WhenStoryIsSpecifiedInReflectiveMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}