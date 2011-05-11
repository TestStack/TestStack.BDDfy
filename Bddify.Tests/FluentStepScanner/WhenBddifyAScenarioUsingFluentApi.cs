using NUnit.Framework;
using Bddify.Core;
using System.Linq;

namespace Bddify.Tests.FluentStepScanner
{
    public class WhenBddifyAScenarioUsingFluentApi
    {
        readonly ScenarioToBeScannerUsingFluentScanner _scenarioInstance = new ScenarioToBeScannerUsingFluentScanner();
        private Core.Story _story;

        [SetUp]
        public void Setup()
        {
            _story = _scenarioInstance.Bddify(ScenarioToBeScannerUsingFluentScanner.GetScanner());
        }

        [Test]
        public void ThenTheStoryShouldBePickedUp()
        {
            Assert.That(_story.MetaData, Is.Not.Null);
            Assert.That(_story.MetaData.Type, Is.EqualTo(typeof(ScenarioToBeScannerUsingFluentScanner)));
        }

        [Test]
        public void ThenTheScenarioShouldBeSetCorrectly()
        {
            Assert.That(_story.Scenarios.Count(), Is.EqualTo(1));
            Assert.That(_story.Scenarios.First().Object.GetType(), Is.EqualTo(typeof(ScenarioToBeScannerUsingFluentScanner)));
        }
    }
}