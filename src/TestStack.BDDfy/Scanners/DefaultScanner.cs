using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class DefaultScanner(ITestContext testContext, IScenarioScanner scenarioScanner, Type? explicitStoryType = null): IScanner
    {
        public Story Scan()
        {
            var scenarios = scenarioScanner.Scan(testContext);
            var metaData = Configurator.Scanners.StoryMetadataScanner().Scan(testContext.TestObject, explicitStoryType);
            return new Story(metaData, [.. scenarios]);
        }
    }
}