using System;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
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
            var scenario = _scenarioScanner.Scan(_testObject);
            var metaData = Configurator.Scanners.StoryMetaDataScanner().Scan(_testObject, _explicitStoryType);

            return new Story(metaData, scenario);
        }
    }
}