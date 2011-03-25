using System;
using System.Reflection;

namespace Bddify.Core
{
    public class ExecutionStep
    {
        public ExecutionStep(MethodInfo method, object[] inputArgs, string readableMethodName, bool asserts, bool shouldReport = true)
        {
            Method = method;
            ReadableMethodName = readableMethodName;
            Asserts = asserts;
            ShouldReport = shouldReport;
            Result = StepExecutionResult.NotExecuted;
            InputArguments = inputArgs;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public MethodInfo Method { get; private set; }
        public object[] InputArguments { get; private set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; set; }
        public string ReadableMethodName { get; private set; }
        public StepExecutionResult Result { get; set; }
        public Exception Exception { get; set; }
    }
}