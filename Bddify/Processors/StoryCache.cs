using System.Collections.Generic;
using Bddify.Core;

namespace Bddify.Processors
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