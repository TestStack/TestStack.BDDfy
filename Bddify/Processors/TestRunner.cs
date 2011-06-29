using Bddify.Core;

namespace Bddify.Processors
{
    public class TestRunner : IProcessor
    {
        public ProcessType ProcessType
        {
            get { return ProcessType.Execute; }
        }

        public void Process(Story story)
        {
            foreach (var scenario in story.Scenarios)
            {
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