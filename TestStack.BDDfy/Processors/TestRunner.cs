namespace TestStack.BDDfy.Processors
{
    using System.Linq;

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
                var executor = new ScenarioExecutor(scenario);
                executor.InitializeScenario();

                foreach (var executionStep in scenario.Steps)
                {
                    if (executor.ExecuteStep(executionStep) == Result.Passed) 
                        continue;

                    if (!scenario.CanContinueExecutionOnStepFail || Configuration.Configurator.Processors.TestRunner.StopExecutionOnFailingThen || !executionStep.Asserts)
                        break;
                }

                if (scenario.Example != null)
                {
                    var unusedValue = scenario.Example.Values.FirstOrDefault(v => !v.ValueHasBeenUsed);
                    if (unusedValue != null) throw new UnusedExampleException(unusedValue);
                }
            }
        }
    }
}