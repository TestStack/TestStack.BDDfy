using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class Scenario
    {
        public Scenario(object testObject, IEnumerable<Step> steps, string scenarioText)
        {
            TestObject = testObject;
            Steps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
            Title = scenarioText;
            Id = Configurator.IdGenerator.GetScenarioId();
        }

        public Scenario(string id, object testObject, IEnumerable<Step> steps, string scenarioText, Example example)
        {
            Id = id;
            TestObject = testObject;
            Steps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
            Title = scenarioText;
            Example = example;
        }

        public string Id { get; set; }
        public string Title { get; private set; }
        public Example Example { get; set; }
        public TimeSpan Duration { get { return new TimeSpan(Steps.Sum(x => x.Duration.Ticks)); } }
        public object TestObject { get; internal set; }
        public List<Step> Steps { get; private set; }

        public Result Result
        {
            get
            {
                if (!Steps.Any())
                    return Result.NotExecuted;

                return (Result)Steps.Max(s => (int)s.Result);
            }
        }
    }
}