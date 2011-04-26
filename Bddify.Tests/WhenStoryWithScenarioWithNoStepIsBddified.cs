using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class WhenStoryWithScenarioWithNoStepIsBddified
    {
        [Story]
        [WithScenario(typeof(EmptyScenario))]
        private class StoryWithOneEmptyScenario
        {
        }

        private class EmptyScenario
        {
            
        }

        [Test]
        public void ThenNoExceptionIsThrown()
        {
            var story = new StoryWithOneEmptyScenario();
            story.Bddify();
        }
    }
}