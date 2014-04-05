using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public Scenario(string id, object testObject, IEnumerable<Step> steps, string scenarioText, string[] exampleHeaders, object[] examples, int exampleRowIndex)
        {
            Id = id;
            TestObject = testObject;
            Steps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
            Title = scenarioText;
            ExampleHeaders = exampleHeaders;
            Examples = examples;
            ExampleRowIndex = exampleRowIndex;
        }

        public string Id { get; set; }
        public string Title { get; private set; }
        public string[] ExampleHeaders { get; set; }
        public object[] Examples { get; set; }
        public int? ExampleRowIndex { get; set; }
        public TimeSpan Duration { get { return new TimeSpan(Steps.Sum(x => x.Duration.Ticks)); } }
        public object TestObject { get; internal set; }
        public List<Step> Steps { get; private set; }
        internal Action<object> Init { get; set; }

        public Result Result
        {
            get
            {
                if (!Steps.Any())
                    return Result.NotExecuted;

                return (Result)Steps.Max(s => (int)s.Result);
            }
        }

        // ToDo: this method does not really belong to this class
        public Result ExecuteStep(Step step)
        {
            try
            {
                if (Init != null)
                    Init(TestObject);
                step.Execute(TestObject);
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