using System.Reflection;
using Bddify.Core;

namespace Bddify.Processors
{
    public class TestRunner : IProcessor
    {
        const string RunScenarioWithArgsMethodName = "RunScenarioWithArgs";

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
                    var argSetterMethod = scenario.TestObject.GetType().GetMethod(RunScenarioWithArgsMethodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    argSetterMethod.Invoke(scenario.TestObject, scenario.ArgsSet);
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