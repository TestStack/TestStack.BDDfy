using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Processors
{
    public class StoryCache : IProcessor
    {
        private static readonly IList<Story> Cache = [];

        public ProcessType ProcessType => ProcessType.Finally;

        public void Process(Story story)
        {
            foreach (var scenario in story.Scenarios.Where(s=>s.TestObject is not null))
            {
                TestContext.ClearContextFor(scenario.TestObject!);
                foreach (var step in scenario.Steps) step.Action = null;
            }

            Cache.Add(story);
        }

        public static IEnumerable<Story> Stories => Cache;
    }
}