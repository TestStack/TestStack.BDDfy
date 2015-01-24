using System;
using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
{
    public class WhenThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            var ex = Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Then, false));
            ex.GetType().FullName.ShouldBe("Gallio.Framework.TestInconclusiveException");
        }

        private void ExecuteUsingFluentScanner()
        {
            var ex = Should.Throw<Exception>(() => Sut.Execute(ThrowingMethods.Then, true));
            ex.GetType().FullName.ShouldBe("Gallio.Framework.TestInconclusiveException");
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
        public void ThenIsReportedAsNotImplemenetedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void ScenarioResultReturnsNoImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNoImplementedWhenUsingReflectingScanners()
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
        public void WhenIsReportedAsSuccessfulWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Fact]
        public void ThenIsReportedAsNotImplemenetedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotImplemented);
        }

        [Fact]
        public void ScenarioResultReturnsNoImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Fact]
        public void StoryResultReturnsNoImplementedWhenUsingFluentScanner()
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