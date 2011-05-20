using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Story
{
    [Story(
        AsA = "As a good programmer",
        IWant = "I want to be able to write my stories as part of my BDD tests",
        SoThat = "So I can get business readable requirements")]
    public class StoryDouble
    {
        [Test]
        public void ScanningAScenarioWithArgsFromAStoryClass()
        {
            var scanner = new DefaultScanner(new ScanForScenarios(new[] { new DefaultScanForStepsByMethodName() }));
            var story = scanner.Scan(typeof(ScenarioInStoryWithArgs));

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.That(story.MetaData.Type, Is.EqualTo(typeof(StoryDouble)));
            Assert.That(story.Scenarios.Count(), Is.EqualTo(2));
            Assert.True(story.Scenarios.All(s => s.TestObject.GetType() == typeof(ScenarioInStoryWithArgs)));
        }

        [Test]
        public void ScanningAScenarioWithoutArgsFromAStoryClass()
        {
            var scanner = new DefaultScanner(new ScanForScenarios(new[] { new DefaultScanForStepsByMethodName() }));
            var story = scanner.Scan(typeof(DummyScenario));

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.That(story.MetaData.Type, Is.EqualTo(typeof(StoryDouble)));
            Assert.That(story.Scenarios.Count(), Is.EqualTo(1));
            Assert.True(story.Scenarios.Single().TestObject.GetType() == typeof(DummyScenario));
        }
    }
}