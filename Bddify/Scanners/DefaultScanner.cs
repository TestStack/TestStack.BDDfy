using System;
using System.Collections.Generic;
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

        public virtual Story Scan(Type storyType)
        {
            var scenarios = GetScenarios(storyType);
            var metaData = GetStoryMetaData(storyType);
            return new Story(metaData, scenarios);
        }

        private IEnumerable<Scenario> GetScenarios(Type storyType)
        {
            return _scenarioScanner.Scan(storyType).ToList();
        }

        StoryMetaData GetStoryMetaData(Type storyType)
        {
            var storyAttribute = GetStoryAttribute(storyType);
            if(storyAttribute == null)
                return ScanAssemblyForStoryMetaData(storyType);

            return new StoryMetaData(storyType, storyAttribute);
        }

        StoryMetaData ScanAssemblyForStoryMetaData(Type scenarioType)
        {
            var assembly = scenarioType.Assembly;
            foreach (var candidateStoryType in assembly.GetTypes())
            {
                var storyAttribute = GetStoryAttribute(candidateStoryType);
                if(storyAttribute == null)
                    continue;

                var withScenariosAttributes = (WithScenarioAttribute[])candidateStoryType.GetCustomAttributes(typeof(WithScenarioAttribute), false);
                if (withScenariosAttributes.Any(a => a.ScenarioType == scenarioType))
                    return new StoryMetaData(candidateStoryType, storyAttribute);
            }

            return null;
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}