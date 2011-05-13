using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions.OtherExceptions
{
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(givenShouldThrow:true));
        }

        [Test]
        public void GivenShouldBeReportedAsFailed()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.Failed));
        }

        [Test]
        public void WhenShouldNotBeExecuted()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
        }

        [Test]
        public void ThenShouldNotBeExecuted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
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
        public void ThenTearDownMethodIsExecuted()
        {
            Assert.That(Sut.TearDownStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }
    }
}