using Bddify.Core;
using Bddify.Processors;
using Bddify.Tests.Exceptions.NotImplementedException;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.NotImplementedException
{
    public class WhenGivenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        public WhenGivenThrowsNotImplementedException()
        {
            Assert.Throws(typeof(InconclusiveException), () => Sut.Execute(givenShouldThrow: true));
        }

        [Fact]
        public void GivenIsReportedAsNotImplemented()
        {
            Assert.Equal(Sut.GivenStep.Result, StepExecutionResult.NotImplemented);
        }

        [Fact]
        public void WhenIsNotExecuted()
        {
            Assert.Equal(Sut.WhenStep.Result, StepExecutionResult.NotExecuted);
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