using System;
using System.Diagnostics;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class Step
    {
        private readonly StepTitle _title;

        public Step(
            Action<object> action,
            StepTitle title,
            bool asserts,
            ExecutionOrder executionOrder,
            bool shouldReport)
        {
            Id = Configurator.IdGenerator.GetStepId();
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = Result.NotExecuted;
            Action = action;
            _title = title;
        }

        public Step(Step step)
        {
            Id = step.Id;
            _title = step._title;
            Asserts = step.Asserts;
            ExecutionOrder = step.ExecutionOrder;
            ShouldReport = step.ShouldReport;
            Result = Result.NotExecuted;
            Action = step.Action;
        }

        public string Id { get; private set; }
        internal Action<object> Action { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string Title
        {
            get
            {
                return _title.ToString();
            }
        }

        public ExecutionOrder ExecutionOrder { get; private set; }

        public Result Result { get; set; }
        public Exception Exception { get; set; }
        public int ExecutionSubOrder { get; set; }
        public TimeSpan Duration { get; set; }

        public void Execute(object testObject)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Action(testObject);
                sw.Stop();
                Duration = sw.Elapsed;
            }
            finally
            {
                sw.Stop();
                Duration = sw.Elapsed;
            }
        }
    }
}