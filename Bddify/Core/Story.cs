using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Story
    {
        public Story(StoryNarrative narrative, Type storyType, IEnumerable<Scenario> scenarios)
        {
            Narrative = narrative;
            Type = storyType;
            Scenarios = scenarios.OrderBy(s => s.ScenarioText).ToList();
        }

        public StoryNarrative Narrative { get; private set; }
        public Type Type { get; private set; }
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