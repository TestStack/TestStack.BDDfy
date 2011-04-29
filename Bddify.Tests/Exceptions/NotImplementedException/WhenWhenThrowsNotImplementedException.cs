using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.NotImplementedException
{
    public class WhenWhenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(whenShouldThrow:true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void WhenIsReportedAsNotImplemented()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
        }

        [Test]
        public void ThenIsNotExecuted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
        }

        [Test]
        public void ThenScenarioResultReturnsNoImplemented()
        {
            Assert.That(Sut.Scenario.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
        }

        [Test]
        public void ThenStoryResultReturnsNoImplemented()
        {
            Assert.That(Sut.Story.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
        }

        [Test]
        public void ThenDisposeMethodIsExecuted()
        {
            Assert.That(Sut.DisposeStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }
    }
}