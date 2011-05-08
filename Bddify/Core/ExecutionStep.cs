using System;
using System.Reflection;
using System.Linq;

namespace Bddify.Core
{
    public class ExecutionStep
    {
        public ExecutionStep(
            MethodInfo method, 
            object[] inputArgs, 
            string readableMethodName, 
            bool asserts, 
            ExecutionOrder executionOrder,
            bool shouldReport = true)
        {
            Method = method;
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = StepExecutionResult.NotExecuted;
            InputArguments = inputArgs;
            Id = Guid.NewGuid();
            ReadableMethodName = readableMethodName;

            if (executionOrder == ExecutionOrder.ConsecutiveAssertion ||
                executionOrder == ExecutionOrder.ConsecutiveSetupState ||
                executionOrder == ExecutionOrder.ConsecutiveTransition)
                ReadableMethodName = "  " + ReadableMethodName; // add two spaces in the front for indentation.
        }

        public Guid Id { get; private set; }
        public MethodInfo Method { get; private set; }
        public object[] InputArguments { get; private set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string ReadableMethodName { get; private set; }
        public StepExecutionResult Result { get; set; }
        public Exception Exception { get; set; }
        public ExecutionOrder ExecutionOrder { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;

            if (GetType() != obj.GetType()) 
                return false;

            var step = (ExecutionStep)obj;

            if(Method != step.Method)
                return false;

            if (InputArguments == null)
                return step.InputArguments == null;

            if(step.InputArguments == null)
                return false;

            return InputArguments.SequenceEqual(step.InputArguments);
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode();
        }
    }
}