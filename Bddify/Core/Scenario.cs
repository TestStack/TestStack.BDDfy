using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Scenario
    {
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

        public StepExecutionResult ExecuteStep(ExecutionStep executionStep)
        {
            try
            {
                executionStep.Method.Invoke(TestObject, executionStep.InputArguments);
                executionStep.Result = StepExecutionResult.Passed;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw;

                if (ex.InnerException is NotImplementedException)
                {
                    executionStep.Result = StepExecutionResult.NotImplemented;
                    executionStep.Exception = ex.InnerException;
                }
                else if (IsInconclusive(ex.InnerException))
                {
                    executionStep.Result = StepExecutionResult.Inconclusive;
                    executionStep.Exception = ex.InnerException;
                }
                else
                {
                    executionStep.Exception = ex.InnerException;
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