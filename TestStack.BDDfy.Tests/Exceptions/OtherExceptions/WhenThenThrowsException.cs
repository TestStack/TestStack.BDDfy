using System;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
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
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Test]
        public void WhenIsReportedAsSuccessfulWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Test]
        public void ThenIsReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.Failed);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.Failed);
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
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Test]
        public void WhenIsReportedAsSuccessfulWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Test]
        public void ThenIsReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.Failed);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}