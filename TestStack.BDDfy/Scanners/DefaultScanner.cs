using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class DefaultScanner : IScanner
    {
        private readonly ITestContext _testContext;
        private readonly Type _explicitStoryType;
        private readonly IScenarioScanner _scenarioScanner;

        public DefaultScanner(ITestContext testContext, IScenarioScanner scenarioScanner, Type explicitStoryType = null)
        {
            this._testContext = testContext;
            _explicitStoryType = explicitStoryType;
            _scenarioScanner = scenarioScanner;
        }

        public Story Scan()
        {
            var scenarios = _scenarioScanner.Scan(_testContext);
            var metaData = Configurator.Scanners.StoryMetadataScanner().Scan(_testContext.TestObject, _explicitStoryType);

            return new Story(metaData, scenarios.ToArray());
        }
    }
}