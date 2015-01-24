using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
{
    public class WhenWhenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            var ex = Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.When, false));
            ex.GetType().FullName.ShouldContain("Inconclusive");
        }

        private void ExecuteUsingFluentScanner()
        {
            var ex = Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.When, true));
            ex.GetType().FullName.ShouldContain("Inconclusive");
        }

        [Fact]
        public void GivenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Fact]
        public void WhenIsReportedAsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void ThenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }

        [Fact]
        public void GivenIsReportedAsSuccessfulWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Fact]
        public void WhenIsReportedAsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Fact]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Fact]
        public void TearDownMethodIsExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}