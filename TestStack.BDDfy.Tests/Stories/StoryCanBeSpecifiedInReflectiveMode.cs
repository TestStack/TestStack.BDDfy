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

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.AreEqual(story.MetaData.Type, typeof(SharedStoryNotion));
        }

        void WhenStoryIsSpecifiedInReflectiveMode()
        {
            
        }

        void ThenTheSpecifiedStoryShouldBeUsed()
        {
        }
    }
}