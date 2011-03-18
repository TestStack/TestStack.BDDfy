using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Bddee
    {
        public Bddee(object bddeeObject, IEnumerable<ExecutionStep> steps, string scenarioSentence)
        {
            Object = bddeeObject;
            _steps = steps.ToList();
            Id = Guid.NewGuid();

            ScenarioSentence = scenarioSentence;
        }

        public string ScenarioSentence { get; private set; }
        public TimeSpan Duration { get; set; }
        public object Object { get; set; }
        public Guid Id { get; private set; }
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