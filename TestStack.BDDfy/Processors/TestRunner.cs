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

                var stepFailed = false;
                foreach (var executionStep in scenario.Steps)
                {
                    if (stepFailed && ShouldExecuteStepWhenPreviousStepFailed(executionStep))
                        break;

                    if (executor.ExecuteStep(executionStep) == Result.Passed) 
                        continue;

                    if (!executionStep.Asserts)
                        break;

                    stepFailed = true;
                }

                if (scenario.Example != null)
                {
                    var unusedValue = scenario.Example.Values.FirstOrDefault(v => !v.ValueHasBeenUsed);
                    if (unusedValue != null) throw new UnusedExampleException(unusedValue);
                }
            }
        }

        private static bool ShouldExecuteStepWhenPreviousStepFailed(Step executionStep)
        {
            return Configuration.Configurator.Processors.TestRunner.StopExecutionOnFailingThen || !executionStep.Asserts;
        }
    }
}