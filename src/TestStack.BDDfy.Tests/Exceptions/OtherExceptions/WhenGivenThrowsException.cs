using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Given, false));
        }

        private void ExecuteUsingFluentScanners()
        {
            
            Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Given, true));
        }

        [Fact]
        public void GivenShouldBeReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(Result.Failed);
        }

        [Fact]
        public void WhenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ThenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
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
        public void GivenShouldBeReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertGivenStepResult(Result.Failed);
        }

        [Fact]
        public void WhenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ThenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Fact]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertStoryResult(Result.Failed);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}