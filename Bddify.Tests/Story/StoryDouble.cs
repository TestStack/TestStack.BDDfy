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
    [TestFixture]
    public class StoryDouble
    {
        [Test]
        public void ScanningAScenarioWithoutArgsFromAStoryClass()
        {
            var scanner = new DefaultScanner(new ScanForScenarios(new[] { new DefaultMethodNameStepScanner() }));
            var story = scanner.Scan(new DummyScenario());

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.That(story.MetaData.Type, Is.EqualTo(typeof(StoryDouble)));
            Assert.That(story.Scenarios.Count(), Is.EqualTo(1));
            Assert.True(story.Scenarios.Single().TestObject.GetType() == typeof(DummyScenario));
        }
    }
}