using NUnit.Framework;
using Bddify.Core;
using System.Linq;

namespace Bddify.Tests.FluentStepScanner
{
    public class WhenBddifyAScenarioUsingFluentApi
    {
        readonly ScenarioToBeScannedUsingFluentScanner _scenarioInstance = new ScenarioToBeScannedUsingFluentScanner();
        private Core.Story _story;

        [SetUp]
        public void Setup()
        {
            _story = _scenarioInstance.Bddify(ScenarioToBeScannedUsingFluentScanner.GetScanner());
        }

        [Test]
        public void ThenTheStoryShouldBePickedUp()
        {
            Assert.That(_story.MetaData, Is.Not.Null);
            Assert.That(_story.MetaData.Type, Is.EqualTo(typeof(ScenarioToBeScannedUsingFluentScanner)));
        }

        [Test]
        public void ThenTheScenarioShouldBeSetCorrectly()
        {
            Assert.That(_story.Scenarios.Count(), Is.EqualTo(1));
            Assert.That(_story.Scenarios.First().Object.GetType(), Is.EqualTo(typeof(ScenarioToBeScannedUsingFluentScanner)));
        }

        [Test]
        public void ThenTheArrayArgumentsArePassedInProperly()
        {
            var instance = (ScenarioToBeScannedUsingFluentScanner)_story.Scenarios.First().Object;
            Assert.True(instance.Input1.SequenceEqual(new[] {"1", "2"}));
            Assert.True(instance.Input2.SequenceEqual(new[] {3, 4}));
            Assert.That(instance.Input3, Is.EqualTo(5));
        }
    }
}