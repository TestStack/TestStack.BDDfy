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

        public virtual Story Scan(object testObject)
        {
            var storyType = testObject.GetType();
            var scenarios = GetScenarios(storyType);
            var narrative = GetStoryNarrative(storyType);
            
            return narrative != null ? new Story(narrative, storyType, scenarios) : new Story(null, null, scenarios);
        }

        private IEnumerable<Scenario> GetScenarios(Type storyType)
        {
            return _scenarioScanner.Scan(storyType).ToList();
        }

        StoryNarrative GetStoryNarrative(Type storyType)
        {
            var storyAttribute = GetStoryAttribute(storyType);
            if(storyAttribute == null)
                return null;

            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.FromTypeName(storyType.Name);

            return new StoryNarrative(title, storyAttribute.AsA, storyAttribute.IWant, storyAttribute.SoThat);
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}