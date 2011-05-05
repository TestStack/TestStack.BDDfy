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
                return null;

            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.Convert(storyType.Name);

            return new StoryMetaData(storyType, title, storyAttribute.AsA, storyAttribute.IWant, storyAttribute.SoThat);
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}