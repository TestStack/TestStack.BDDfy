using System;
namespace Bddify.Core
{
    public class Story
    {
        public Story(StoryAttribute narrative, Type storyType)
        {
            Narrative = narrative;
            StoryType = storyType;
        }

        public StoryAttribute Narrative { get; set; }
        public Type StoryType { get; set; }
    }
}