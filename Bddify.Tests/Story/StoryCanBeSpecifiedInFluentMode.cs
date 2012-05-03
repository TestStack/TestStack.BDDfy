using Bddify.Core;
using Bddify.Scanners.StepScanners.Fluent;
using NUnit.Framework;

namespace Bddify.Tests.Story
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
                .Bddify<SharedStoryNotion>();

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