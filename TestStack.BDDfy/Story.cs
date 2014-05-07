using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Story
    {
        public Story(StoryMetadata metadata, params Scenario[] scenarios)
        {
            Metadata = metadata;
            Scenarios = scenarios;

            if (scenarios.Length > 0)
            {
                var testObject = scenarios.First().TestObject;
                if(testObject != null)
                    Namespace = testObject.GetType().Namespace;
            }
        }

        public StoryMetadata Metadata { get; private set; }
        
        /// <summary>
        /// Currently used only when scenario doesn't have a story and we use the namespace instead
        /// </summary>
        public string Namespace { get; set; }
        public string ReportFilename { get; set; }
        public IEnumerable<Scenario> Scenarios { get; private set; }

        public Result Result
        {
            get 
            {
                return (Result)Scenarios.Max(s => (int)s.Result); 
            }
        }
    }
}