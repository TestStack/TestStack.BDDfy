using System;
using System.Text;

namespace TestStack.BDDfy.Core
{
    public class StoryMetaData
    {
        // ReSharper disable InconsistentNaming
        private const string I_want_prefix = "I want";
        private const string So_that_prefix = "So that";
        private const string As_a_prefix = "As a";
        // ReSharper restore InconsistentNaming

        public StoryMetaData(Type storyType, StoryAttribute storyAttribute)
        {
            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.Convert(storyType.Name);

            Type = storyType;
            Title = title;

            AsA = CleanseProperty(storyAttribute.AsA, As_a_prefix);
            IWant= CleanseProperty(storyAttribute.IWant, I_want_prefix);
            SoThat = CleanseProperty(storyAttribute.SoThat, So_that_prefix);
        }

        public StoryMetaData(Type storyType, string asA, string iWant, string soThat, string storyTitle = null)
        {
            Title = storyTitle ?? NetToString.Convert(storyType.Name);
            Type = storyType;

            AsA = CleanseProperty(asA, As_a_prefix);
            IWant = CleanseProperty(iWant, I_want_prefix);
            SoThat = CleanseProperty(soThat, So_that_prefix);
        }

        string CleanseProperty(string text, string prefix)
        {
            var property = new StringBuilder();

            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                property.AppendFormat("{0} ", prefix);

            property.Append(text);
            return property.ToString();
        }

        public Type Type { get; private set; }
        public string Title { get; private set; }
        public string AsA { get; private set; }
        public string IWant { get; private set; }
        public string SoThat { get; private set; }
    }
}