using Bddify.Core;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Story
{
    public class WhenScannerIsRunOnAStory
    {
        private StoryDouble _story;
        private Bddifier _bddifier;

        [SetUp]
        public void Setup()
        {
            _story = new StoryDouble();
            _bddifier = _story.LazyBddify();
            _bddifier.Run();
        }

        [Test]
        public void AllScenariosAreFound()
        {
            Assert.That(_bddifier.Story.Scenarios.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ThenStoryScenarioTypesAreFound()
        {
            Assert.That(_bddifier.Story.Scenarios.GroupBy(s => s.Object.GetType()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryItselfIsNotReturnedAsAScenario()
        {
            Assert.That(_bddifier.Story.Scenarios.Any(s => s.Object.GetType() == typeof(StoryDouble)), Is.False);
        }

        [Test]
        public void ThenRunScenarioWithArgsReturnsOneScenarioPerAttribute()
        {
            var scenariosWithArgs = _bddifier.Story.Scenarios.Where(s => s.Object.GetType() == typeof(ScenarioInStoryWithArgs));
            Assert.That(scenariosWithArgs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryTypeIsSetOnStory()
        {
            Assert.That(_bddifier.Story.Type, Is.EqualTo(typeof(StoryDouble)));
        }

        [Test]
        public void ThenStoryNarrativeIsSetOnStory()
        {
            var narrative = (StoryAttribute)typeof(StoryDouble).GetCustomAttributes(typeof(StoryAttribute), false)[0];
            Assert.That(_bddifier.Story.Narrative, Is.Not.Null);
            Assert.That(_bddifier.Story.Narrative.AsA, Is.EqualTo(narrative.AsA));
            Assert.That(_bddifier.Story.Narrative.IWant, Is.EqualTo(narrative.IWant));
            Assert.That(_bddifier.Story.Narrative.SoThat, Is.EqualTo(narrative.SoThat));
        }
    }
}