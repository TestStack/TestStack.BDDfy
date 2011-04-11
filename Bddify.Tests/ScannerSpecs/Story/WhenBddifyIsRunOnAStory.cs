using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.ScannerSpecs.Story
{
    public class WhenBddifyIsRunOnAStory
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
    }
}