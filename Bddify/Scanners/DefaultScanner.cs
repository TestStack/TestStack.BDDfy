using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class DefaultScanner : IScanner
    {
        private readonly IScanForScenarios _scenarioScanner;

        public DefaultScanner(IScanForScenarios scenarioScanner)
        {
            _scenarioScanner = scenarioScanner;
        }

        public virtual Story Scan(Type scenarioType)
        {
            var scenarios = GetScenarios(scenarioType);
            var metaData = GetStoryMetaData(scenarioType);
            return new Story(metaData, scenarios);
        }

        private IEnumerable<Scenario> GetScenarios(Type scenarioType)
        {
            return _scenarioScanner.Scan(scenarioType).ToList();
        }

        StoryMetaData GetStoryMetaData(Type scenarioType)
        {
            var storyAttribute = GetStoryAttribute(scenarioType);
            if(storyAttribute == null)
                return GetStoryMetaDataByWalkingUpTheCallStack(scenarioType);

            return new StoryMetaData(scenarioType, storyAttribute);
        }

        StoryMetaData GetStoryMetaDataByWalkingUpTheCallStack(Type scenarioType)
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            if(frames == null)
                return null;

            // This is assuming scenario and story live in the same assembly
            var firstFrame = frames.Reverse().FirstOrDefault(f => f.GetMethod().DeclaringType.Assembly == scenarioType.Assembly);
            if(firstFrame == null)
                return null;

            var candidateStoryType = firstFrame.GetMethod().DeclaringType;
            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if(storyAttribute == null)
                return null;

            return new StoryMetaData(candidateStoryType, storyAttribute);
        }

        internal StoryAttribute GetStoryAttribute(Type scenarioType)
        {
            return (StoryAttribute)scenarioType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}