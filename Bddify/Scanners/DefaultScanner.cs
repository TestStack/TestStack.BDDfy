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

        static Dictionary<Type, StoryMetaData> _scenarioToStoryMapper;
        static readonly object MapperSyncRoot = new object();

        StoryMetaData ScanAssemblyForStoryMetaData(Type scenarioType)
        {
            lock (MapperSyncRoot)
            {
                if (_scenarioToStoryMapper == null)
                {
                    _scenarioToStoryMapper = new Dictionary<Type, StoryMetaData>();

                    var assembly = scenarioType.Assembly;
                    foreach (var candidateStoryType in assembly.GetTypes())
                    {
                        var storyAttribute = GetStoryAttribute(candidateStoryType);
                        if (storyAttribute == null)
                            continue;

                        var withScenariosAttributes = (WithScenarioAttribute[])candidateStoryType.GetCustomAttributes(typeof(WithScenarioAttribute), false);
                        foreach (var withScenarioAttribute in withScenariosAttributes)
                            _scenarioToStoryMapper.Add(withScenarioAttribute.ScenarioType, new StoryMetaData(candidateStoryType, storyAttribute));
                    }
                }
            }

            StoryMetaData storyType;
            if (_scenarioToStoryMapper.TryGetValue(scenarioType, out storyType))
                return storyType;

            return null;
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}