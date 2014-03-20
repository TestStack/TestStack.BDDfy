using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
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
            Sut.AssertGivenStepResult(Result.Passed);
        }

        [Test]
        public void WhenIsReportedAsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotImplemented);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
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
        public void WhenIsReportedAsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.NotImplemented);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
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