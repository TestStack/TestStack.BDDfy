using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Tests.Configuration;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class ComplexStepsTests
    {
        private int count;

        [Test]
        public void ShouldBeAbleToChainComplexTestWithFluentApi()
        {
            this.Given(_ => count.ShouldBe(0))
                .When(() => count++.ShouldBe(0), "When I do something")
                .Given(() => count++.ShouldBe(1), "Given I am doing things in different order")
                .Then(() => count++.ShouldBe(2), "Then they should run in defined order")
                .When(() => count++.ShouldBe(3), "When I have whens after thens things still work")
                .And(() => count++.ShouldBe(4), "And we should still be able to use ands")
                .BDDfy();
        }
        
        [Test]
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

            Assert.Throws<Exception>(() => testRun.Run());
            var scenario = testRun.Story.Scenarios.First();
            Assert.AreEqual(Result.Failed, scenario.Result);
            var steps = scenario.Steps;

            Assert.AreEqual(6, steps.Count);
            Assert.AreEqual(Result.Passed, steps[0].Result);
            Assert.AreEqual(ExecutionOrder.SetupState, steps[0].ExecutionOrder);
            Assert.AreEqual(Result.Passed, steps[1].Result);
            Assert.AreEqual(ExecutionOrder.Transition, steps[1].ExecutionOrder);
            Assert.AreEqual(Result.Failed, steps[2].Result);
            Assert.AreEqual(ExecutionOrder.Assertion, steps[2].ExecutionOrder);
            Assert.AreEqual(Result.Passed, steps[3].Result);
            Assert.AreEqual(ExecutionOrder.ConsecutiveAssertion, steps[3].ExecutionOrder);
            Assert.AreEqual(Result.NotExecuted, steps[4].Result);
            Assert.AreEqual(ExecutionOrder.Transition, steps[4].ExecutionOrder);
            Assert.AreEqual(Result.NotExecuted, steps[5].Result);
            Assert.AreEqual(ExecutionOrder.Assertion, steps[5].ExecutionOrder);
        }
    }
}