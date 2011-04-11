using Bddify.Core;
using Bddify.Tests.ScannerSpecs.Story;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.ScannerSpecs
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
        public void ThenStoryScenarioTypesAreFound()
        {
            Assert.That(_bddifier.Scenarios.GroupBy(s => s.Object.GetType()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryItselfIsNotReturnedAsAScenario()
        {
            Assert.That(_bddifier.Scenarios.Any(s => s.Object.GetType() == typeof(StoryDouble)), Is.False);
        }

        [Test]
        public void ThenScenarioTypeWithArgsReturnsTwoScenarios()
        {
            var scenariosWithArgs = _bddifier.Scenarios.Where(s => s.Object.GetType() == typeof(ScenarioInStoryWithArgs));
            Assert.That(scenariosWithArgs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ThenStoryTypeShouldBeSetOnScenarios()
        {
            Assert.True(_bddifier.Scenarios.All(s => s.Story == typeof(StoryDouble)));
        }
    }
}