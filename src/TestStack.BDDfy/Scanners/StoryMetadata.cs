using System;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class StoryMetadata(Type storyType, string narrative1, string narrative2, string narrative3, string title = null, string titlePrefix = null, string imageUri = null, string storyUri = null)
    {
        public StoryMetadata(Type storyType, StoryNarrativeAttribute narrative)
            : this(storyType, narrative.Narrative1, narrative.Narrative2, narrative.Narrative3, narrative.Title, narrative.TitlePrefix, narrative.ImageUri, narrative.StoryUri)
        {
        }

        public Type Type { get; private set; } = storyType;
        public string Title { get; private set; } = title ?? Configurator.Humanizer.Humanize(storyType.Name);
        public string TitlePrefix { get; private set; } = titlePrefix ?? "Story: ";
        public string Narrative1 { get; private set; } = narrative1;
        public string Narrative2 { get; private set; } = narrative2;
        public string Narrative3 { get; private set; } = narrative3;
        public string ImageUri { get; private set; } = imageUri;
        public string StoryUri { get; private set; } = storyUri;
    }
}