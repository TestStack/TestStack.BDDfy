using System;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Configuration
{
    public class TestRunnerTests
    {
        public class ScenarioWithFailingThen
        {
            [Given]
            public void PassingGiven()
            {
            }

            [When]
            public void PassingWhen()
            {
            }

            [Then]
            public void FailingThen()
            {
                throw new Exception();
            }

            [AndThen]
            public void PassingAndThen()
            {
            }
        }


        [Collection(TestCollectionName.ModifiesConfigurator)]

        public class When_StopExecutionOnFailingThen_IsSetToTrue
        {
            [Fact]
            public void FailingThenStopsThePipelineWithReflectiveAPI()
            {
                Configurator.Processors.TestRunner.StopExecutionOnFailingThen = true;
    
                try
                {
                    var testRun = new ScenarioWithFailingThen().LazyBDDfy();
                    Verify(testRun);
                }
                finally
                {
                    Configurator.Processors.TestRunner.StopExecutionOnFailingThen = false;
                }
            }

            [Fact]
            public void FailingThenStopsThePipelineWithFluentAPI()
            {
                Configurator.Processors.TestRunner.StopExecutionOnFailingThen = true;
    
                try
                {
                    var testRun = new ScenarioWithFailingThen()
                        .Given(x => x.PassingGiven())
                        .When(x => x.PassingWhen())
                        .Then(x => x.FailingThen())
                        .And(x => x.PassingAndThen())
                        .LazyBDDfy();

                    Verify(testRun);
                }
                finally
                {
                    Configurator.Processors.TestRunner.StopExecutionOnFailingThen = false;
                }
            }

            private static void Verify(Engine testRun)
            {
                Should.Throw<Exception>(() => testRun.Run());
                var scenario = testRun.Story.Scenarios.First();
                scenario.Result.ShouldBe(Result.Failed);
                var steps = scenario.Steps;

                steps.Count.ShouldBe(4);
                steps[0].Result.ShouldBe(Result.Passed);
                steps[0].ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
                steps[1].Result.ShouldBe(Result.Passed);
                steps[1].ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
                steps[2].Result.ShouldBe(Result.Failed);
                steps[2].ExecutionOrder.ShouldBe(ExecutionOrder.Assertion);
                steps[3].Result.ShouldBe(Result.NotExecuted);
                steps[3].ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
            }
        }

        public class When_StopExecutionOnFailingThen_IsLeftAsDefault
        {
            [Fact]
            public void FailingThenDoesNotStopThePipelineWithReflectiveAPI()
            {
                var testRun = new ScenarioWithFailingThen().LazyBDDfy();
                Verify(testRun);
            }

            [Fact]
            public void FailingThenDoesNotStopThePipelineWithFluentAPI()
            {
                Configurator.Processors.TestRunner.StopExecutionOnFailingThen = false;

                var testRun = new ScenarioWithFailingThen()
                    .Given(x => x.PassingGiven())
                    .When(x => x.PassingWhen())
                    .Then(x => x.FailingThen())
                    .And(x => x.PassingAndThen())
                    .LazyBDDfy();

                Verify(testRun);
            }

            private static void Verify(Engine testRun)
            {
                Should.Throw<Exception>(() => testRun.Run());
                var scenario = testRun.Story.Scenarios.First();
                scenario.Result.ShouldBe(Result.Failed);
                var steps = scenario.Steps;

                steps.Count.ShouldBe(4);
                steps[0].Result.ShouldBe(Result.Passed);
                steps[0].ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
                steps[1].Result.ShouldBe(Result.Passed);
                steps[1].ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
                steps[2].Result.ShouldBe(Result.Failed);
                steps[2].ExecutionOrder.ShouldBe(ExecutionOrder.Assertion);
                steps[3].Result.ShouldBe(Result.Passed);
                steps[3].ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
            }
        }
    }
}
