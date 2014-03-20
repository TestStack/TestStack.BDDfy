using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
{
    public class WhenThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        private void ExecuteUsingReflectingScanners()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Then, false));
        }

        private void ExecuteUsingFluentScanner()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(ThrowingMethods.Then, true));
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
        public void ThenIsReportedAsNotImplemenetedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotImplemented);
        }

        [Test]
        public void ScenarioResultReturnsNoImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNoImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.NotImplemented);
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
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Test]
        public void WhenIsReportedAsSuccessfulWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.Passed);
        }

        [Test]
        public void ThenIsReportedAsNotImplemenetedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotImplemented);
        }

        [Test]
        public void ScenarioResultReturnsNoImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNoImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Test]
        public void TearDownMethodIsExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertTearDownMethodIsExecuted();
        }
    }
}