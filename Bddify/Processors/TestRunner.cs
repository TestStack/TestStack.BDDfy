using System.Reflection;
using Bddify.Core;

namespace Bddify.Processors
{
    public class TestRunner : IProcessor
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

        public void Process(Story story)
        {
            foreach (var scenario in story.Scenarios)
            {
                if (scenario.ArgsSet != null)
                {
                    var argSetterMethod = scenario.Object.GetType().GetMethod(_runScenarioWithArgsMethodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    argSetterMethod.Invoke(scenario.Object, scenario.ArgsSet);
                }

                foreach (var executionStep in scenario.Steps)
                {
                    if (scenario.ExecuteStep(executionStep) == StepExecutionResult.Passed) 
                        continue;

                    if(!executionStep.Asserts)                        
                        break;
                }
            }
        }
    }
}