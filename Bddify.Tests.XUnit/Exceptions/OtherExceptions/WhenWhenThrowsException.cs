using System;
using Bddify.Core;
using Bddify.Tests.Exceptions.OtherExceptions;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.OtherExceptions
{
    public class WhenWhenThrowsException : OtherExceptionBase
    {
        public WhenWhenThrowsException()
        {
            Assert.Throws(typeof(Exception), () => Sut.Execute(whenShouldThrow: true));
        }

        [Fact]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.Equal(Sut.GivenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void WhenIsReportedAsFailed()
        {
            Assert.Equal(Sut.WhenStep.Result, StepExecutionResult.Failed);
        }

        [Fact]
        public void ThenIsNotExecuted()
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