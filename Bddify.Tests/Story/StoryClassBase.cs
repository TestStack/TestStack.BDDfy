using Bddify.Core;

namespace Bddify.Tests.Story
{
    [Story(
        Title = StoryTitle,
        AsA = "As a programmer",
        IWant = "I want bddify to inherit StoryAttribute",
        SoThat = "So that I can put some shared logic on a base story class")]
    public abstract class StoryClassBase
    {
        protected const string StoryTitle = "Story base class";

        protected void GivenTheStoryAttributeIsSetOnTheBaseClass()
        {
            
        }
    }
}