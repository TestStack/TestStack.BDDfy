using System.Collections.Generic;

namespace TestStack.BDDfy.Processors
{
    public class StoryCache : IProcessor
    {
        private static readonly IList<Story> Cache = new List<Story>();

        public ProcessType ProcessType
        {
            get { return ProcessType.Finally; }
        }

        public void Process(Story story)
        {
            foreach (var scenario in story.Scenarios)
            {
                scenario.TestObject = null;
                foreach (var step in scenario.Steps)
                    step.Action = null;
            }

            Cache.Add(story);
        }

        public static IEnumerable<Story> Stories
        {
            get
            {
                return Cache;
            }
        }
    }
}