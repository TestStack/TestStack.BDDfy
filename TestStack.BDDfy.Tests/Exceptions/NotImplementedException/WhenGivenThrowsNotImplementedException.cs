using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
{
    public class WhenGivenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingFluentScanner()
        {
            Should.Throw<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Given, true));
        }

        private void ExecuteUsingReflectingScanners()
        {
            Should.Throw<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Given, false));
        }

        [Fact]
        public void GivenIsReportedAsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void GivenIsReportedAsNotImplementedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void WhenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void WhenIsNotExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ThenIsNotExecutedWhenUsingReflectingScanner()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}