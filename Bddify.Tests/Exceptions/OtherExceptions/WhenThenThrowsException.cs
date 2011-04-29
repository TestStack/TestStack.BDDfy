using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.OtherExceptions
{
    public class WhenThenThrowsException : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(thenShouldThrow : true));
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
        public void ThenIsReportedAsFailed()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.Failed));
        }

        [Test]
        public void ThenScenarioResultReturnsFailed()
        {
            Assert.That(Sut.Scenario.Result, Is.EqualTo(StepExecutionResult.Failed));
        }

        [Test]
        public void ThenStoryResultReturnsFailed()
        {
            Assert.That(Sut.Scenario.Result, Is.EqualTo(StepExecutionResult.Failed));
        }

        [Test]
        public void ThenDisposeMethodIsExecuted()
        {
            Assert.That(Sut.DisposeStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }
    }
}