using System;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class StoryMetadata
    {
        public StoryMetadata(Type storyType, StoryNarrativeAttribute narrative)
            : this(storyType, narrative.Narrative1, narrative.Narrative2, narrative.Narrative3, narrative.Title, narrative.TitlePrefix, narrative.ImageUri, narrative.StoryUri)
        {
        }

        public StoryMetadata(Type storyType, string narrative1, string narrative2, string narrative3, string title = null, string titlePrefix = null, string imageUri = null, string storyUri = null)
        {
            Title = title ?? Configurator.Scanners.Humanize(storyType.Name);
            TitlePrefix = titlePrefix ?? "Story: ";
            Type = storyType;

            Narrative1 = narrative1;
            Narrative2 = narrative2;
            Narrative3 = narrative3;

            ImageUri = imageUri;
            StoryUri = storyUri;
        }

        public Type Type { get; private set; }
        public string Title { get; private set; }
        public string TitlePrefix { get; private set; }
        public string Narrative1 { get; private set; }
        public string Narrative2 { get; private set; }
        public string Narrative3 { get; private set; }
        public string ImageUri { get; private set; }
        public string StoryUri { get; private set; }
    }
}