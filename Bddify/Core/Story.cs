using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Story
    {
        public Story(StoryMetaData metaData, params Scenario[] scenarios)
        {
            MetaData = metaData;
            Scenarios = scenarios.OrderBy(s => s.ScenarioText).ToList();
        }

        public StoryMetaData MetaData { get; private set; }
        public IEnumerable<Scenario> Scenarios { get; private set; }

        public StepExecutionResult Result
        {
            get 
            {
                return (StepExecutionResult)Scenarios.Max(s => (int)s.Result); 
            }
        }
    }
}