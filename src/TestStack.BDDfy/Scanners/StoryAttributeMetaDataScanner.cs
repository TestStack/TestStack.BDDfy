using System;
using System.Linq;
using System.Reflection;

namespace TestStack.BDDfy.Scanners
{
    internal class StoryAttributeMetadataScanner : IStoryMetadataScanner
    {
        public virtual StoryMetadata? Scan(object testObject, Type? explicitStoryType = null) 
            => GetStoryMetadata(testObject, explicitStoryType) ?? GetStoryMetadataFromScenario(testObject);

        static StoryMetadata? GetStoryMetadataFromScenario(object testObject)
        {
            var scenarioType = testObject.GetType();
            var storyAttribute = GetStoryAttribute(scenarioType);
            if (storyAttribute is null) return null;

            return new StoryMetadata(scenarioType, storyAttribute);
        }

        StoryMetadata? GetStoryMetadata(object testObject, Type? explicitStoryType)
        {
            var candidateStoryType = GetCandidateStory(testObject, explicitStoryType);
            if (candidateStoryType is null) return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if (storyAttribute is null) return null;

            return new StoryMetadata(candidateStoryType, storyAttribute);
        }

        protected virtual Type? GetCandidateStory(object testObject, Type? explicitStoryType)
        {
            if (explicitStoryType != null)
                return explicitStoryType;

            var testObjectType = testObject.GetType();
            var declaringType = testObjectType.DeclaringType;
            return declaringType ?? testObjectType;
        }

        static StoryNarrativeAttribute? GetStoryAttribute(Type candidateStoryType) 
            => candidateStoryType.GetCustomAttribute<StoryNarrativeAttribute>(true);
    }
}