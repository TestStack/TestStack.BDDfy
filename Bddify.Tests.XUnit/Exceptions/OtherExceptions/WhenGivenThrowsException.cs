using System;
using Bddify.Core;
using Bddify.Tests.Exceptions;
using Bddify.Tests.Exceptions.OtherExceptions;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.OtherExceptions
{
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        public WhenGivenThrowsException()
        {
            Assert.Throws(typeof(Exception), () => Sut.Execute(ThrowingMethod.Given));
        }

        [Fact]
        public void GivenShouldBeReportedAsFailed()
        {
            Assert.Equal(Sut.GivenStep.Result, StepExecutionResult.Failed);
        }

        [Fact]
        public void WhenShouldNotBeExecuted()
        {
            Assert.Equal(Sut.WhenStep.Result, StepExecutionResult.NotExecuted);
        }

        [Fact]
        public void ThenShouldNotBeExecuted()
        {
            Assert.Equal(Sut.ThenStep.Result, StepExecutionResult.NotExecuted);
        }

        [Fact]
        public void ThenScenarioResultReturnsFailed()
        {
            Assert.Equal(Sut.Scenario.Result, StepExecutionResult.Failed);
        }

        [Fact]
        public void ThenStoryResultReturnsFailed()
        {
            Assert.Equal(Sut.Scenario.Result, StepExecutionResult.Failed);
        }
    }
}