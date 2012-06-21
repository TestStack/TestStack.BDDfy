using NUnit.Framework;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Tests.Story
{
    [Story]
    public class StoryCanBeSpecifiedInReflectiveMode
    {
        [Test] 
        public void Verift()
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