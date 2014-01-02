using System;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

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

        [TestFixture]
        public class When_StopExecutionOnFailingThen_IsSetToTrue
        {
            [Test]
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

            [Test]
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
                Assert.Throws<Exception>(() => testRun.Run());
                var scenario = testRun.Story.Scenarios.First();
                Assert.AreEqual(StepExecutionResult.Failed, scenario.Result);
                var steps = scenario.Steps;

                Assert.AreEqual(4, steps.Count);
                Assert.AreEqual(StepExecutionResult.Passed, steps[0].Result);
                Assert.AreEqual(ExecutionOrder.SetupState, steps[0].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.Passed, steps[1].Result);
                Assert.AreEqual(ExecutionOrder.Transition, steps[1].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.Failed, steps[2].Result);
                Assert.AreEqual(ExecutionOrder.Assertion, steps[2].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.NotExecuted, steps[3].Result);
                Assert.AreEqual(ExecutionOrder.ConsecutiveAssertion, steps[3].ExecutionOrder);
            }
        }

        [TestFixture]
        public class When_StopExecutionOnFailingThen_IsLeftAsDefault
        {
            [Test]
            public void FailingThenDoesNotStopThePipelineWithReflectiveAPI()
            {
                var testRun = new ScenarioWithFailingThen().LazyBDDfy();
                Verify(testRun);
            }

            [Test]
            public void FailingThenDoesNotStopThePipelineWithFluentAPI()
            {
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
                Assert.Throws<Exception>(() => testRun.Run());
                var scenario = testRun.Story.Scenarios.First();
                Assert.AreEqual(StepExecutionResult.Failed, scenario.Result);
                var steps = scenario.Steps;

                Assert.AreEqual(4, steps.Count);
                Assert.AreEqual(StepExecutionResult.Passed, steps[0].Result);
                Assert.AreEqual(ExecutionOrder.SetupState, steps[0].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.Passed, steps[1].Result);
                Assert.AreEqual(ExecutionOrder.Transition, steps[1].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.Failed, steps[2].Result);
                Assert.AreEqual(ExecutionOrder.Assertion, steps[2].ExecutionOrder);
                Assert.AreEqual(StepExecutionResult.Passed, steps[3].Result);
                Assert.AreEqual(ExecutionOrder.ConsecutiveAssertion, steps[3].ExecutionOrder);
            }
        }
    }
}
