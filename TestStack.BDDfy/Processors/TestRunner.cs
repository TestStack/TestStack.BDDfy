namespace TestStack.BDDfy.Processors
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
                var executor = new ScenarioExecutor(scenario);
                executor.InitializeScenario();

                foreach (var executionStep in scenario.Steps)
                {
                    if (executor.ExecuteStep(executionStep) == Result.Passed) 
                        continue;

                    if (Configuration.Configurator.Processors.TestRunner.StopExecutionOnFailingThen || !executionStep.Asserts)
                        break;
                }
            }
        }
    }
}