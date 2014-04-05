using System;
using System.Diagnostics;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class Step
    {
        private readonly Func<object, string> _titleFactory;
        private string _title;

        public Step(
            Action<object> action,
            Func<object, string> titleFactory, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport)
        {
            _titleFactory = titleFactory;
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = Result.NotExecuted;
            Action = action;
            Id = Configurator.IdGenerator.GetStepId();
        }

        public string Id { get; private set; }
        internal Action<object> Action { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public Result Result { get; set; }
        public Exception Exception { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }
        public int ExecutionSubOrder { get; set; }
        public TimeSpan Duration { get; set; }

        public string Title
        {
            get
            {
                if (_title == null)
                    return _titleFactory(null);
                return _title;
            }
        }

        public void Execute(object testObject)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                _title = _titleFactory(testObject);
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