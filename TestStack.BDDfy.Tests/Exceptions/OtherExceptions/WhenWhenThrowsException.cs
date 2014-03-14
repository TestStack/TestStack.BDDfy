using System;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenWhenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingFluentScanner()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.When, true));
        }

        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.When, false));
        }

        [Test]
        public void GivenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void GivenIsReportedAsSuccessfulWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void WhenIsReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.Failed);
        }

        [Test]
        public void WhenIsReportedAsFailedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(StepExecutionResult.Failed);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(StepExecutionResult.Failed);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(StepExecutionResult.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(StepExecutionResult.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(StepExecutionResult.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}