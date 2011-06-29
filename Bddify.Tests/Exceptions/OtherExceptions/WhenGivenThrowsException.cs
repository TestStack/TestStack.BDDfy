using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.OtherExceptions
{
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Given, false));
        }

        private void ExecuteUsingFluentScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Given, true));
        }

        [Test]
        public void GivenShouldBeReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.Failed);
        }

        [Test]
        public void WhenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ThenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(StepExecutionResult.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(StepExecutionResult.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Test]
        public void GivenShouldBeReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.Failed);
        }

        [Test]
        public void WhenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ThenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertScenarioResult(StepExecutionResult.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertStoryResult(StepExecutionResult.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}