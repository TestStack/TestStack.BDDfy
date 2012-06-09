using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors
{
    public class Disposer : IProcessor
    {
        public ProcessType ProcessType
        {
            get { return ProcessType.Finally; }
        }

        public void Process(Story story)
        {
            foreach (var scenario in story.Scenarios)
            {
                Dispose(scenario);
            }
        }

        /// <summary>
        /// Runs all the dispose methods in the scenario
        /// </summary>
        /// <param name="scenario"></param>
        private static void Dispose(Scenario scenario)
        {
            var disposeSteps = scenario
                .Steps
                .Where(s => s.ExecutionOrder == ExecutionOrder.TearDown && s.Result == StepExecutionResult.NotExecuted);
            
            foreach (var disposeStep in disposeSteps)
            {
                scenario.ExecuteStep(disposeStep);
            }
        }

    }
}