using System;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.ScenarioScanners;

namespace Bddify.Scanners
{
    public class DefaultScanner : IScanner
    {
        private readonly IScanForScenarios _scenarioScanner;

        public DefaultScanner(IScanForScenarios scenarioScanner)
        {
            _scenarioScanner = scenarioScanner;
        }

        public virtual Story Scan(object testObject)
        {
            var scenario = GetScenario(testObject);
            var metaData = GetStoryMetaData(testObject.GetType());
            return new Story(metaData, scenario);
        }

        private Scenario GetScenario(object testObject)
        {
            return _scenarioScanner.Scan(testObject);
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