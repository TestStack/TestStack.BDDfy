using System;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Given, false));
        }

        private void ExecuteUsingFluentScanners()
        {
            Assert.Throws<Exception>(() => Sut.Execute(ThrowingMethods.Given, true));
        }

        [Test]
        public void GivenShouldBeReportedAsFailedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertGivenStepResult(Result.Failed);
        }

        [Test]
        public void WhenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ThenShouldNotBeExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
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
        public void GivenShouldBeReportedAsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertGivenStepResult(Result.Failed);
        }

        [Test]
        public void WhenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ThenShouldNotBeExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertScenarioResult(Result.Failed);
        }

        [Test]
        public void StoryResultReturnsFailedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertStoryResult(Result.Failed);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanners();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}