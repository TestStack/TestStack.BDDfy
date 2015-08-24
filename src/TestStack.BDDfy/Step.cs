using System;
using System.Collections.Generic;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class Step
    {
        private readonly StepTitle _title;

        public Step(
            Func<object, object> action,
            StepTitle title,
            bool asserts,
            ExecutionOrder executionOrder,
            bool shouldReport, 
            List<StepArgument> arguments)
        {
            Id = Configurator.IdGenerator.GetStepId();
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = Result.NotExecuted;
            Action = action;
            Arguments = arguments;
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
            Arguments = step.Arguments;
        }

        public string Id { get; private set; }
        internal Func<object, object> Action { get; set; }
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
        public List<StepArgument> Arguments { get; private set; }
    }
}