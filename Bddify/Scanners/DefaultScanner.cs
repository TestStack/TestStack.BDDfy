using System;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.ScenarioScanners;

namespace Bddify.Scanners
{
    public class DefaultScanner : IScanner
    {
        private readonly IScenarioScanner _scenarioScanner;
        private readonly object _testObject;

        public DefaultScanner(object testObject, IScenarioScanner scenarioScanner)
        {
            _scenarioScanner = scenarioScanner;
            _testObject = testObject;
        }

        public Story Scan()
        {
            var scenario = GetScenario();
            var metaData = GetStoryMetaDataFromScenario() ?? GetStoryMetaData();

            return new Story(metaData, scenario);
        }

        private Scenario GetScenario()
        {
            return _scenarioScanner.Scan(_testObject);
        }

        StoryMetaData GetStoryMetaDataFromScenario()
        {
            var scenarioType = _testObject.GetType();
            var storyAttribute = GetStoryAttribute(scenarioType);
            if(storyAttribute == null)
                return null;

            return new StoryMetaData(scenarioType, storyAttribute);
        }

        StoryMetaData GetStoryMetaData()
        {
            var candidateStoryType = GetCandidateStory();
            if(candidateStoryType == null)
                return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if(storyAttribute == null)
                return null;

            return new StoryMetaData(candidateStoryType, storyAttribute);
        }

        protected virtual Type GetCandidateStory()
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            if (frames == null)
                return null;

            var scenarioType = _testObject.GetType();
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