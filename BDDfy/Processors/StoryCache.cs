using System.Collections.Generic;
using BDDfy.Core;

namespace BDDfy.Processors
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