using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Scenario
    {
        public Scenario(object testClass, IEnumerable<ExecutionStep> steps, string scenarioSentence, IEnumerable<object[]> argSets = null)
        {
            Object = testClass;
            _steps = steps.ToList();
            Id = Guid.NewGuid();

            ScenarioSentence = scenarioSentence;
            _argSests = argSets;
        }

        public string ScenarioSentence { get; private set; }
        public TimeSpan Duration { get; set; }
        public object Object { get; set; }
        public Guid Id { get; private set; }
        private IEnumerable<object[]> _argSests;
        public IEnumerable<object[]> ArgSets
        {
            get { return _argSests; }
        }

        private readonly List<ExecutionStep> _steps;
        public IEnumerable<ExecutionStep> Steps
        {
            get { return _steps; }
        }

        public StepExecutionResult Result
        {
            get
            {
                return (StepExecutionResult)Steps.Max(s => (int)s.Result);
            }
        }
    }
}