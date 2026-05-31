using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Story
    {
        public Story(StoryMetadata? metadata, params Scenario[] scenarios)
        {
            Metadata = metadata;
            Scenarios = scenarios;
            Namespace = metadata?.Type?.Namespace ?? "Tests";

            if (scenarios.Length == 0) return;

            var storyNamespace = scenarios.First().TestObject?.GetType()?.Namespace;
            if (string.IsNullOrWhiteSpace(storyNamespace)) return;
            Namespace = storyNamespace;
        }

        public StoryMetadata? Metadata { get; }

        /// <summary>
        /// Currently used only when scenario doesn't have a story and we use the namespace instead
        /// </summary>
        public string Namespace { get; set; }
        public IEnumerable<Scenario> Scenarios { get; private set; }

        public Result Result => (Result)Scenarios.Max(s => (int)s.Result);
    }
}