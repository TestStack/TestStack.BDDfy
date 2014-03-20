using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestStack.BDDfy
{
    public class Scenario
    {
        public Scenario(object testObject, IEnumerable<Step> steps, string scenarioText)
        {
            TestObject = testObject;
            _steps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
            Id = Guid.NewGuid();

            Title = scenarioText;
        }

        public string Title { get; private set; }
        public TimeSpan Duration { get { return new TimeSpan(_steps.Sum(x => x.Duration.Ticks)); } }
        public object TestObject { get; internal set; }
        public Guid Id { get; private set; }

        private readonly List<Step> _steps;
        public List<Step> Steps
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
        public StepExecutionResult ExecuteStep(Step step)
        {
            try
            {
                step.Execute(TestObject);
                step.Result = StepExecutionResult.Passed;
            }
            catch (Exception ex)
            {
                // ToDo: more thought should be put into this. Is it safe to get the exception?
                var exception = ex;
                if (exception is TargetInvocationException)
                {
                    exception = ex.InnerException ?? ex;
                }

                if (exception is NotImplementedException)
                {
                    step.Result = StepExecutionResult.NotImplemented;
                    step.Exception = exception;
                }
                else if (IsInconclusive(exception))
                {
                    step.Result = StepExecutionResult.Inconclusive;
                    step.Exception = exception;
                }
                else
                {
                    step.Exception = exception;
                    step.Result = StepExecutionResult.Failed;
                }
            }

            return step.Result;
        }

        private static bool IsInconclusive(Exception exception)
        {
            return exception.GetType().Name.Contains("InconclusiveException");
        }
    }
}