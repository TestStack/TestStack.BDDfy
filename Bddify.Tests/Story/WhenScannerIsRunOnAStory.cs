using System.Linq;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests.Story
{
    public class WhenScannerIsRunOnAStory
    {
        private Core.Story _story;

        [SetUp]
        public void Setup()
        {
            var story = new StoryDouble();
            var scanner = new DefaultScanner(new ScanForScenarios(new DefaultScanForStepsByMethodName()));
            _story = scanner.Scan(story);
        }

        [Test]
        public void AllScenariosAreFound()
        {
            Assert.That(_story.Scenarios.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ThenStoryScenarioTypesAreFound()
        {
            Assert.That(_story.Scenarios.GroupBy(s => s.Object.GetType()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryItselfIsNotReturnedAsAScenario()
        {
            Assert.That(_story.Scenarios.Any(s => s.Object.GetType() == typeof(StoryDouble)), Is.False);
        }

        [Test]
        public void ThenRunScenarioWithArgsReturnsOneScenarioPerAttribute()
        {
            var scenariosWithArgs = _story.Scenarios.Where(s => s.Object.GetType() == typeof(ScenarioInStoryWithArgs));
            Assert.That(scenariosWithArgs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryTypeIsSetOnStory()
        {
            Assert.That(_story.Type, Is.EqualTo(typeof(StoryDouble)));
        }

        [Test]
        public void ThenStoryNarrativeIsSetOnStory()
        {
            var narrative = (StoryAttribute)typeof(StoryDouble).GetCustomAttributes(typeof(StoryAttribute), false)[0];
            Assert.That(_story.Narrative, Is.Not.Null);
            Assert.That(_story.Narrative.AsA, Is.EqualTo(narrative.AsA));
            Assert.That(_story.Narrative.IWant, Is.EqualTo(narrative.IWant));
            Assert.That(_story.Narrative.SoThat, Is.EqualTo(narrative.SoThat));
        }
    }
}