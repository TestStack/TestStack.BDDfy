using BDDfy.Core;
using NUnit.Framework;

namespace BDDfy.Tests.Exceptions.NotImplementedException
{
    public class WhenWhenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.When, false));
        }

        private void ExecuteUsingFluentScanner()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.When, true));
        }

        [Test]
        public void GivenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void WhenIsReportedAsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Test]
        public void GivenIsReportedAsSuccessfulWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(StepExecutionResult.Passed);
        }

        [Test]
        public void WhenIsReportedAsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}