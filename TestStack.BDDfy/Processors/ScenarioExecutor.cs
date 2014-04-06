using System;
using System.Reflection;

namespace TestStack.BDDfy.Processors
{
    public class ScenarioExecutor
    {
        private readonly Scenario _scenario;

        public ScenarioExecutor(Scenario scenario)
        {
            _scenario = scenario;
        }

        public void InitializeScenario()
        {
            if (_scenario.Init != null)
                _scenario.Init(_scenario.TestObject);
        }

        public Result ExecuteStep(Step step)
        {
            try
            {
                step.Execute(_scenario.TestObject);
                step.Result = Result.Passed;
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
                    step.Result = Result.NotImplemented;
                    step.Exception = exception;
                }
                else if (IsInconclusive(exception))
                {
                    step.Result = Result.Inconclusive;
                    step.Exception = exception;
                }
                else
                {
                    step.Exception = exception;
                    step.Result = Result.Failed;
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
