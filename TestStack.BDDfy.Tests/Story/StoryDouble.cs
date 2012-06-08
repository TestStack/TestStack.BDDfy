using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;
using TestStack.BDDfy.Scanners.ScenarioScanners;
using TestStack.BDDfy.Scanners.StepScanners.MethodName;

namespace TestStack.BDDfy.Tests.Story
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
            var scanner = new DefaultScanner(testObject, new ReflectiveScenarioScanner(new[] { new DefaultMethodNameStepScanner() }));
            var story = scanner.Scan();

            Assert.That(story.MetaData, Is.Not.Null);
            Assert.That(story.MetaData.Type, Is.EqualTo(typeof(StoryDouble)));
            Assert.That(story.Scenarios.Count(), Is.EqualTo(1));
            Assert.True(story.Scenarios.Single().TestObject.GetType() == typeof(DummyScenario));
        }
    }
}