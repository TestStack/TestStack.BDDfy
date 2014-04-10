using System;
using System.Linq;
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
            if (_scenario.Example == null) 
                return;

            foreach (var column in _scenario.Example)
            {
                var type = _scenario.TestObject.GetType();
                var matchingMembers = type.GetMembers()
                    .Where(m => m is FieldInfo || m is PropertyInfo)
                    .Where(n => n.Name.Equals(column.Key, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();

                if (!matchingMembers.Any())
                    continue;

                foreach (var matchingMember in matchingMembers)
                {
                    var prop = matchingMember as PropertyInfo;
                    if (prop != null)
                        prop.SetValue(_scenario.TestObject, column.Value, null);

                    var field = matchingMember as FieldInfo;
                    if (field != null)
                        field.SetValue(_scenario.TestObject, column.Value);
                }
            }
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
