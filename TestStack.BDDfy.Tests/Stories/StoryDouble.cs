using System.Linq;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Stories
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
            var testObject = new DummyScenario();
            var scanner = new DefaultScanner(TestContext.GetContext(testObject), new ReflectiveScenarioScanner(new[] { new DefaultMethodNameStepScanner() }));
            var story = scanner.Scan();

            Assert.That(story.Metadata, Is.Not.Null);
            Assert.That(story.Metadata.Type, Is.EqualTo(typeof(StoryDouble)));
            Assert.That(story.Scenarios.Count(), Is.EqualTo(1));
            Assert.True(story.Scenarios.Single().TestObject.GetType() == typeof(DummyScenario));
        }
    }
}