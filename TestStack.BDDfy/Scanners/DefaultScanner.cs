using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class DefaultScanner : IScanner
    {
        private readonly Type _explicitStoryType;
        private readonly IScenarioScanner _scenarioScanner;
        private readonly object _testObject;

        public DefaultScanner(object testObject, IScenarioScanner scenarioScanner, Type explicitStoryType = null)
        {
            _explicitStoryType = explicitStoryType;
            _scenarioScanner = scenarioScanner;
            _testObject = testObject;
        }

        public Story Scan()
        {
            var scenarios = _scenarioScanner.Scan(_testObject);
            var metaData = Configurator.Scanners.StoryMetadataScanner().Scan(_testObject, _explicitStoryType);

            return new Story(metaData, scenarios.ToArray());
        }
    }
}