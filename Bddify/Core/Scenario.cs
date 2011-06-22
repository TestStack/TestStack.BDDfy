using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Scenario
    {
#if NET35
        public Scenario(object testObject, IEnumerable<ExecutionStep> steps, string scenarioText)
            :this(testObject, steps, scenarioText, null)
        {
        }
#endif

        public Scenario(object testObject, IEnumerable<ExecutionStep> steps, string scenarioText, object[] argsSet = null)
        {
            TestObject = testObject;
            _steps = steps.OrderBy(o => o.ExecutionOrder).ToList();
            Id = Guid.NewGuid();

            ScenarioText = scenarioText;
            _argsSet = argsSet;
        }

        public string ScenarioText { get; private set; }
        public TimeSpan Duration { get; set; }
        public object TestObject { get; set; }
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

        // ToDo: this method does not really belong to this class
        public StepExecutionResult ExecuteStep(ExecutionStep executionStep)
        {
            try
            {
                executionStep.Execute(TestObject);
                executionStep.Result = StepExecutionResult.Passed;
            }
            catch (Exception ex)
            {
                // ToDo: more thought should be put into this. Is it safe to get the exception?
                var exception = ex.InnerException ?? ex;

                if (exception is NotImplementedException)
                {
                    executionStep.Result = StepExecutionResult.NotImplemented;
                    executionStep.Exception = exception;
                }
                else if (IsInconclusive(exception))
                {
                    executionStep.Result = StepExecutionResult.Inconclusive;
                    executionStep.Exception = exception;
                }
                else
                {
                    executionStep.Exception = exception;
                    executionStep.Result = StepExecutionResult.Failed;
                }
            }

            return executionStep.Result;
        }

        private static bool IsInconclusive(Exception exception)
        {
            return exception.GetType().Name.Contains("InconclusiveException");
        }
    }
}