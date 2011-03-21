using System;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Processors
{
    public class TestRunner<TInconclusiveException> : IProcessor
        where TInconclusiveException : Exception
    {
        private readonly string _runScenarioWithArgsMethodName;

        public TestRunner(string runScenarioWithArgsMethodName = "RunScenarioWithArgs")
        {
            _runScenarioWithArgsMethodName = runScenarioWithArgsMethodName;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Execute; }
        }

        public void Process(Scenario scenario)
        {
            if (scenario.ArgsSet != null)
            {
                var argSetterMethod = scenario.Object.GetType().GetMethod(_runScenarioWithArgsMethodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                argSetterMethod.Invoke(scenario.Object, scenario.ArgsSet);
            }

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