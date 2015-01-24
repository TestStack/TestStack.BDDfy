using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenThenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Then, false));
        }

        private void ExecuteUsingFluentScanner()
        {
            Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Then, true));
        }

        [Fact]
        public void GivenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Fact]
        public void WhenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Fact]
        public void ThenIsReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.Failed);
        }

        [Fact]
        public void ScenarioResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Fact]
        public void StoryResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.Failed);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Fact]
        public void GivenIsReportedAsSuccessfulWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Fact]
        public void WhenIsReportedAsSuccessfulWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Fact]
        public void ThenIsReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.Failed);
        }

        [Fact]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Fact]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.Failed);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}