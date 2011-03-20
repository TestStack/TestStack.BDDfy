using System;
using Bddify.Core;

namespace Bddify.Processors
{
    public class TestRunner<TInconclusiveException> : IProcessor
        where TInconclusiveException : Exception
    {
        public ProcessType ProcessType
        {
            get { return ProcessType.Execute; }
        }

        public void Process(Scenario scenario)
        {
            foreach (var executionStep in scenario.Steps)
            {
                try
                {
                    executionStep.Method.Invoke(scenario.Object, executionStep.InputArguments);
                    executionStep.Result = StepExecutionResult.Passed;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                        throw;

                    if (ex.InnerException is NotImplementedException)
                    {
                        executionStep.Result = StepExecutionResult.NotImplemented;
                        executionStep.Exception = ex.InnerException;
                    }
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