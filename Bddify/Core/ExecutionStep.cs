using System;
using System.Linq;
using System.Reflection;

namespace Bddify.Core
{
    public class ExecutionStep
    {
        public ExecutionStep(
            string readableMethodName, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport)
        {
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = StepExecutionResult.NotExecuted;
            Id = Guid.NewGuid();
            ReadableMethodName = readableMethodName;

            if (executionOrder == ExecutionOrder.ConsecutiveAssertion ||
                executionOrder == ExecutionOrder.ConsecutiveSetupState ||
                executionOrder == ExecutionOrder.ConsecutiveTransition)
                ReadableMethodName = "  " + ReadableMethodName; // add two spaces in the front for indentation.
        }

        public ExecutionStep(
            MethodInfo stepMethod,
            object[] stepMethodInputArguments,
            string readableMethodName, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport) : this(readableMethodName, asserts, executionOrder, shouldReport)
        {
            _stepMethod = stepMethod;
            _inputArguments = stepMethodInputArguments;
            StepAction = testObject => stepMethod.Invoke(testObject, _inputArguments);
        }

        public ExecutionStep(
            Action<object> stepAction,
            string readableMethodName, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport) : this(readableMethodName, asserts, executionOrder, shouldReport)
        {
            StepAction = stepAction;
        }

        // these two variables are kept only for equality comparison
        readonly object[] _inputArguments;
        readonly MethodInfo _stepMethod;

        public Guid Id { get; private set; }
        Action<object> StepAction { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string ReadableMethodName { get; private set; }
        public StepExecutionResult Result { get; set; }
        public Exception Exception { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }

        public void Execute(object testObject)
        {
            StepAction(testObject);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;

            if (GetType() != obj.GetType()) 
                return false;

            var step = (ExecutionStep)obj;

            // this step is created using Action instead of MethodInfo + Input Arguments
            // so we do not quite care about their equality, and there is no way to find out if they are equal or not
            if (_stepMethod == null)
                return false;

            if (_stepMethod != step._stepMethod)
                return false;

            if (_inputArguments == null)
                return step._inputArguments == null;

            if (step._inputArguments == null)
                return false;

            return _inputArguments.SequenceEqual(step._inputArguments);
        }

        public override int GetHashCode()
        {
            return ReadableMethodName.GetHashCode();
        }
    }
}