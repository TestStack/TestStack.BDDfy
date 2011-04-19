using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Scenario
    {
        public Scenario(object testClass, IEnumerable<ExecutionStep> steps, string scenarioText, object[] argsSet = null)
        {
            Object = testClass;
            _steps = GetUniqueSteps(steps);
            Id = Guid.NewGuid();

            ScenarioText = scenarioText;
            _argsSet = argsSet;
        }

        static List<ExecutionStep> GetUniqueSteps(IEnumerable<ExecutionStep> steps)
        {
            return steps.Distinct().OrderBy(o => o.ExecutionOrder).ToList();
        }

        public string ScenarioText { get; private set; }
        public TimeSpan Duration { get; set; }
        public object Object { get; set; }
        public Guid Id { get; private set; }
        private object[] _argsSet;
        public object[] ArgsSet
        {
            get { return _argsSet; }
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
                if (!Steps.Any())
                    return StepExecutionResult.NotExecuted;

                return (StepExecutionResult)Steps.Max(s => (int)s.Result);
            }
        }
    }
}