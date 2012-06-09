using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Core
{
    public class Story
    {
        public Story(StoryMetaData metaData, params Scenario[] scenarios)
        {
            MetaData = metaData;
            Scenarios = scenarios.OrderBy(s => s.Title).ToList();
        }

        public StoryMetaData MetaData { get; private set; }
        public string Category { get; set; }
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