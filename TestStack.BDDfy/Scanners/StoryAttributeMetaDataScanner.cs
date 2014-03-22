using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestStack.BDDfy
{
    public class StoryAttributeMetadataScanner : IStoryMetadataScanner
    {
        // ReSharper disable InconsistentNaming
        private const string I_want_prefix = "I want";
        private const string So_that_prefix = "So that";
        private const string As_a_prefix = "As a";
        private const string In_order_to_prefix = "In order to";
        // ReSharper restore InconsistentNaming

        public virtual StoryMetadata Scan(object testObject, Type explicitStoryType = null)
        {
            return GetStoryMetadata(testObject, explicitStoryType) ?? GetStoryMetadataFromScenario(testObject);
        }

        static StoryMetadata GetStoryMetadataFromScenario(object testObject)
        {
            var scenarioType = testObject.GetType();
            var storyAttribute = GetStoryAttribute(scenarioType);
            if (storyAttribute == null)
                return null;

            return CreateStoryMetadata(scenarioType, storyAttribute);
        }

        StoryMetadata GetStoryMetadata(object testObject, Type explicityStoryType)
        {
            var candidateStoryType = GetCandidateStory(testObject, explicityStoryType);
            if (candidateStoryType == null)
                return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if (storyAttribute == null)
                return null;

            return CreateStoryMetadata(candidateStoryType, storyAttribute);
        }

        static StoryMetadata CreateStoryMetadata(Type storyType, StoryAttribute storyAttribute)
        {
            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.Convert(storyType.Name);

            string narrative1, narrative2, narrative3;

            if (!string.IsNullOrWhiteSpace(storyAttribute.InOrderTo))
            {
                narrative1 = CleanseProperty(storyAttribute.InOrderTo, In_order_to_prefix);
                narrative2 = CleanseProperty(storyAttribute.AsA, As_a_prefix);
                narrative3 = CleanseProperty(storyAttribute.IWant, I_want_prefix);
            }
            else
            {
                narrative1 = CleanseProperty(storyAttribute.AsA, As_a_prefix);
                narrative2 = CleanseProperty(storyAttribute.IWant, I_want_prefix);
                narrative3 = CleanseProperty(storyAttribute.SoThat, So_that_prefix);
            }

            return new StoryMetadata(storyType, narrative1, narrative2, narrative3, title);
        }

        static string CleanseProperty(string text, string prefix)
        {
            var property = new StringBuilder();

            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                property.AppendFormat("{0} ", prefix);

            property.Append(text);
            return property.ToString();
        }

        protected virtual Type GetCandidateStory(object testObject, Type explicitStoryType)
        {
            if (explicitStoryType != null)
                return explicitStoryType;

            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            if (frames == null)
                return null;

            var scenarioType = testObject.GetType();
            // This is assuming scenario and story live in the same assembly
            var firstFrame = frames.LastOrDefault(f => f.GetMethod().DeclaringType.Assembly == scenarioType.Assembly);
            if (firstFrame == null)
                return null;

            return firstFrame.GetMethod().DeclaringType;
        }

        static StoryAttribute GetStoryAttribute(Type candidateStoryType)
        {
            return (StoryAttribute)candidateStoryType.GetCustomAttributes(typeof(StoryAttribute), true).FirstOrDefault();
        }
    }
}