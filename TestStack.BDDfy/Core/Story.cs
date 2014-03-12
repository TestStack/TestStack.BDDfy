using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Core
{
    public class Story
    {
        public Story(StoryMetaData metaData, params Scenario[] scenarios)
        {
            MetaData = metaData;
            Scenarios = scenarios;

            if (scenarios.Length > 0)
            {
                var testObject = scenarios.First().TestObject;
                if(testObject != null)
                    Namespace = testObject.GetType().Namespace;
            }
        }

        public StoryMetaData MetaData { get; private set; }
        
        /// <summary>
        /// Currently used only when scenario doesn't have a story and we use the namespace instead
        /// </summary>
        public string Namespace { get; set; }
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