using System;

namespace Bddify.Core
{
    public class ExecutionStep
    {
        public ExecutionStep(
            Action<object> stepAction,
            string stepTitle, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport)
        {
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = StepExecutionResult.NotExecuted;
            Id = Guid.NewGuid();
            StepTitle = stepTitle;
            StepAction = stepAction;
        }

        public Guid Id { get; private set; }
        Action<object> StepAction { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string StepTitle { get; private set; }
        public StepExecutionResult Result { get; set; }
        public Exception Exception { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }

        public void Execute(object testObject)
        {
            StepAction(testObject);
        }
    }
}