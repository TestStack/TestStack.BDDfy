using System;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Tests.Concurrency;
using TestStack.BDDfy.Tests.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class ComplexStepsTests
    {
        private int count;

        [Fact]
        public void ShouldBeAbleToChainComplexTestWithFluentApi()
        {
            this.Given(_ => count.ShouldBe(0, "count should start with 0"))
                .When(() => count++.ShouldBe(0), "When I do something")
                .Given(() => count++.ShouldBe(1), "Given I am doing things in different order")
                .Then(() => count++.ShouldBe(2), "Then they should run in defined order")
                .When(() => count++.ShouldBe(3), "When I have whens after thens things still work")
                .And(() => count++.ShouldBe(4), "And we should still be able to use ands")
                .BDDfy();
        }
        
        [Fact]
        public void ShouldContinueExecutingThensButStopWhenNextNotAssertStepIsHit()
        {
            var testRun = new TestRunnerTests.ScenarioWithFailingThen()
                    .Given(x => x.PassingGiven())
                    .When(x => x.PassingWhen())
                    .Then(x => x.FailingThen())
                    .And(x => x.PassingAndThen())
                    .When(x => x.PassingWhen())
                    .Then(x => x.FailingThen())
                    .LazyBDDfy();

            Should.Throw<Exception>(() => testRun.Run());
            var scenario = testRun.Story.Scenarios.First();
            scenario.Result.ShouldBe(Result.Failed);
            var steps = scenario.Steps;

            steps.Count.ShouldBe(6);
            steps[0].Result.ShouldBe(Result.Passed);
            steps[0].ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
            steps[1].Result.ShouldBe(Result.Passed);
            steps[1].ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
            steps[2].Result.ShouldBe(Result.Failed);
            steps[2].ExecutionOrder.ShouldBe(ExecutionOrder.Assertion);
            steps[3].Result.ShouldBe(Result.Passed);
            steps[3].ExecutionOrder.ShouldBe(ExecutionOrder.ConsecutiveAssertion);
            steps[4].Result.ShouldBe(Result.NotExecuted);
            steps[4].ExecutionOrder.ShouldBe(ExecutionOrder.Transition);
            steps[5].Result.ShouldBe(Result.NotExecuted);
            steps[5].ExecutionOrder.ShouldBe(ExecutionOrder.Assertion);
        }
    }
}