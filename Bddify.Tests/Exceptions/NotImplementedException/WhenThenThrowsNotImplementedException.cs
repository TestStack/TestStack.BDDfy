using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.NotImplementedException
{
    public class WhenThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(thenShouldThrow:true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void WhenIsReportedAsSuccessful()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void ThenIsReportedAsNotImplemeneted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
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
        public void ThenTearDownMethodIsExecuted()
        {
            Assert.That(Sut.TearDownStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }
    }
}