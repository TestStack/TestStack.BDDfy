using System;

namespace Bddify
{
    public class TestRunner<TInconclusiveException> : ITestRunner
        where TInconclusiveException : Exception
    {
        public void Run(Bddee bddee)
        {
            foreach (var executionStep in bddee.Steps)
            {
                try
                {
                    executionStep.Method.Invoke(bddee.Object, null);
                    executionStep.Result = StepExecutionResult.Succeeded;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                        throw;

                    if (ex.InnerException is NotImplementedException)
                        executionStep.Result = StepExecutionResult.NotImplemented;
                    else if (ex.InnerException.GetType() == typeof(TInconclusiveException))
                    {
                        executionStep.Result = StepExecutionResult.Inconclusive;
                        executionStep.Exception = ex.InnerException;
                    }
                    else
                    {
                        executionStep.Exception = ex.InnerException;
                        executionStep.Result = StepExecutionResult.Failed;
                    }

                    // exceptions are only tolerated on asserting methods
                    if(!executionStep.Asserts)
                        break;
                }
            }
        }
    }
}