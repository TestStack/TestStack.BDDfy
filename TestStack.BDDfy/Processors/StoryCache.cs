using System.Collections.Generic;
using TestStack.BDDfy.Core;

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
                // ToDo: should uncomment this and find a way to generate file reports with null TestObject.
                // Currently it fails when there is no story as the namespace is resolved through TestObject
                // scenario.TestObject = null;

                foreach (var step in scenario.Steps)
                    step.StepAction = null;
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