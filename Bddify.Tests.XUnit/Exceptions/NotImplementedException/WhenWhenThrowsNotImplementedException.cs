using Bddify.Core;
using Bddify.Processors;
using Bddify.Tests.Exceptions;
using Bddify.Tests.Exceptions.NotImplementedException;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.NotImplementedException
{
    public class WhenWhenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        public WhenWhenThrowsNotImplementedException()
        {
            Assert.Throws(typeof(InconclusiveException), () => Sut.Execute(ThrowingMethod.When));
        }

        [Fact]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.Equal(Sut.GivenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void WhenIsReportedAsNotImplemented()
        {
            Assert.Equal(Sut.WhenStep.Result, StepExecutionResult.NotImplemented);
        }

        [Fact]
        public void ThenIsNotExecuted()
        {
            Assert.Equal(Sut.ThenStep.Result, StepExecutionResult.NotExecuted);
        }

        [Fact]
        public void ThenScenarioResultReturnsNoImplemented()
        {
            Assert.Equal(Sut.Scenario.Result, StepExecutionResult.NotImplemented);
        }

        [Fact]
        public void ThenStoryResultReturnsNoImplemented()
        {
            Assert.Equal(Sut.Story.Result, StepExecutionResult.NotImplemented);
        }
    }
}