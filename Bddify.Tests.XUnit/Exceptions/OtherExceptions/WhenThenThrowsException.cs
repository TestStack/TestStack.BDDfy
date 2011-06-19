using System;
using Bddify.Core;
using Bddify.Tests.Exceptions;
using Bddify.Tests.Exceptions.OtherExceptions;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.OtherExceptions
{
    public class WhenThenThrowsException : OtherExceptionBase
    {
        public WhenThenThrowsException()
        {
            Assert.Throws(typeof(Exception), () => Sut.Execute(ThrowingMethod.Then));
        }

        [Fact]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.Equal(Sut.GivenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void WhenIsReportedAsSuccessful()
        {
            Assert.Equal(Sut.WhenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void ThenIsReportedAsFailed()
        {
            Assert.Equal(Sut.ThenStep.Result, StepExecutionResult.Failed);
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