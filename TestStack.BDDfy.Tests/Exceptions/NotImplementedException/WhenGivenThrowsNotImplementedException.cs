using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.NotImplementedException
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
            Sut.AssertGivenStepResult(Result.NotImplemented);
        }

        [Test]
        public void GivenIsReportedAsNotImplementedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertGivenStepResult(Result.NotImplemented);
        }

        [Test]
        public void WhenIsNotExecutedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Test]
        public void WhenIsNotExecutedWhenUsingFluentScanners()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertWhenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingReflectingScanner()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ThenIsNotExecutedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertThenStepResult(Result.NotExecuted);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void ScenarioResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertScenarioResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingReflectingScanners()
        {
            ExecuteUsingReflectingScanners();
            Sut.AssertStoryResult(Result.NotImplemented);
        }

        [Test]
        public void StoryResultReturnsNotImplementedWhenUsingFluentScanner()
        {
            ExecuteUsingFluentScanner();
            Sut.AssertStoryResult(Result.NotImplemented);
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