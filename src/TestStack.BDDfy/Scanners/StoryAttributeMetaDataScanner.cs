using System;
using System.Diagnostics;
using System.Linq;

namespace TestStack.BDDfy
{
    public class StoryAttributeMetadataScanner : IStoryMetadataScanner
    {
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

            return new StoryMetadata(scenarioType, storyAttribute);
        }

        StoryMetadata GetStoryMetadata(object testObject, Type explicityStoryType)
        {
            var candidateStoryType = GetCandidateStory(testObject, explicityStoryType);
            if (candidateStoryType == null)
                return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if (storyAttribute == null)
                return null;

            return new StoryMetadata(candidateStoryType, storyAttribute);
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
            var firstFrame = frames.LastOrDefault(f => f.GetMethod().DeclaringType?.Assembly == scenarioType.Assembly);
            if (firstFrame == null)
                return null;

            return firstFrame.GetMethod().DeclaringType;
        }

        static StoryNarrativeAttribute GetStoryAttribute(Type candidateStoryType)
        {
            return (StoryNarrativeAttribute)candidateStoryType.GetCustomAttributes(typeof(StoryNarrativeAttribute), true).FirstOrDefault();
        }
    }
}