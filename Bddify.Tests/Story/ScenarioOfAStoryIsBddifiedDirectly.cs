using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests.Story
{
    public class ScenarioOfAStoryIsBddifiedDirectly
    {
        private Core.Story _story;

        void WhenScenarioOfAStoryIsBddifiedDirectly()
        {
            var scanner = new DefaultScanner(new ScanForScenarios(new[] { new DefaultScanForStepsByMethodName() }));
            _story = scanner.Scan(typeof(ScenarioInStoryWithArgs));
        }

        void ThenStoryMetaDataIsFound()
        {
            Assert.That(_story.MetaData, Is.Not.Null);
        }

        void ThenStoryMetaDataHasCorrectType()
        {
            Assert.That(_story.MetaData.Type, Is.EqualTo(typeof(StoryDouble)));
        }

        [Test]
        public void Execute()
        {
            WhenScenarioOfAStoryIsBddifiedDirectly();
            ThenStoryMetaDataIsFound();
            ThenStoryMetaDataHasCorrectType();
        }
    }
}