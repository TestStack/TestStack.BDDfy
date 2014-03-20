using System;
using System.Diagnostics;

namespace TestStack.BDDfy
{
    public class Step
    {
        public Step(
            Action<object> action,
            string title, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport)
        {
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = Result.NotExecuted;
            Id = Guid.NewGuid();
            Title = title;
            Action = action;
        }

        public Guid Id { get; private set; }
        internal Action<object> Action { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string Title { get; private set; }
        public Result Result { get; set; }
        public Exception Exception { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }
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