using System;

namespace Bddify.Core
{
    public class StoryMetaData
    {
        public StoryMetaData(Type storyType, string title, string asA, string want, string soThat)
        {
            Type = storyType;
            Title = title;
            AsA = asA;
            IWant = want;
            SoThat = soThat;
        }

        public Type Type { get; private set; }
        public string Title { get; private set; }
        public string AsA { get; private set; }
        public string IWant { get; private set; }
        public string SoThat { get; private set; }
    }
}