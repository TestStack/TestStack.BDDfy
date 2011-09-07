using System;
using Bddify.Scanners.ScenarioScanners;

namespace Bddify.Scanners
{
    public class DefaultScanner<TStory> : DefaultScanner
        where TStory : class
    {
        public DefaultScanner(object testObject, IScenarioScanner scenarioScanner) :
            base(testObject, scenarioScanner)
        {
        }

        protected override Type GetCandidateStory()
        {
            return typeof(TStory);
        }
    }
}