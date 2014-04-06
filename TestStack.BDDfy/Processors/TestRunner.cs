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
                if (scenario.Init != null)
                    scenario.Init(scenario.TestObject);

                foreach (var executionStep in scenario.Steps)
                {
                    if (scenario.ExecuteStep(executionStep) == Result.Passed) 
                        continue;

                    if (Configuration.Configurator.Processors.TestRunner.StopExecutionOnFailingThen || !executionStep.Asserts)
                        break;
                }
            }
        }
    }
}