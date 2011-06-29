using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.OtherExceptions
{
    public class WhenThenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Then, false));
        }

        private void ExecuteUsingFluentScanner()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Then, true));
        }

        [Test]
        public void GivenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void WhenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void ThenIsReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(StepExecutionResult.Failed);
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
        public void GivenIsReportedAsSuccessfulWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void WhenIsReportedAsSuccessfulWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void ThenIsReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(StepExecutionResult.Failed);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(StepExecutionResult.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(StepExecutionResult.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}