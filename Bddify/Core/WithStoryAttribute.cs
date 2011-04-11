using System;

namespace Bddify.Core
{
    public class WithStoryAttribute : Attribute
    {
        public Type StoryType { get; private set; }

        public WithStoryAttribute(Type storyType)
        {
            StoryType = storyType;
        }
    }
}