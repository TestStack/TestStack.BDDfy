using System;
using System.Diagnostics;
using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
{
    public class StoryAttributeMetaDataScanner : IStoryMetaDataScanner
    {
        public virtual StoryMetaData Scan(object testObject, Type explicitStoryType = null)
        {
            return GetStoryMetaData(testObject, explicitStoryType) ?? GetStoryMetaDataFromScenario(testObject);
        }

        static StoryMetaData GetStoryMetaDataFromScenario(object testObject)
        {
            var scenarioType = testObject.GetType();
            var storyAttribute = GetStoryAttribute(scenarioType);
            if (storyAttribute == null)
                return null;

            return new StoryMetaData(scenarioType, storyAttribute);
        }

        StoryMetaData GetStoryMetaData(object testObject, Type explicityStoryType)
        {
            var candidateStoryType = GetCandidateStory(testObject, explicityStoryType);
            if (candidateStoryType == null)
                return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if (storyAttribute == null)
                return null;

            return new StoryMetaData(candidateStoryType, storyAttribute);
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