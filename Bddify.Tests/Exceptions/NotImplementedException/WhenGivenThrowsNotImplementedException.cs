using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.NotImplementedException
{
    public class WhenGivenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingFluentScanner()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Given, true));
        }

        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Given, false));            
        }

        [Test]
        public void GivenIsReportedAsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void GivenIsReportedAsNotImplementedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void WhenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void WhenIsNotExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(StepExecutionResult.NotExecuted);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingReflectingScanner()
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
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(StepExecutionResult.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(StepExecutionResult.NotImplemented);
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