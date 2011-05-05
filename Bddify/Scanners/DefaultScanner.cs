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

            var title = GetTitle(storyType, storyAttribute);

            return new StoryMetaData(storyType, title, storyAttribute.AsA, storyAttribute.IWant, storyAttribute.SoThat);
        }

        private static string GetTitle(Type storyType, StoryAttribute storyAttribute)
        {
            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.Convert(storyType.Name);

            return title;
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
                {
                    var title = GetTitle(candidateStoryType, storyAttribute);
                    return new StoryMetaData(candidateStoryType, title, storyAttribute.AsA, storyAttribute.IWant, storyAttribute.SoThat);
                }
            }

            return null;
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}