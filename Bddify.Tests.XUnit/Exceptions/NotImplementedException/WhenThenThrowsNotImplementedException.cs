using Bddify.Core;
using Bddify.Processors;
using Bddify.Tests.Exceptions.NotImplementedException;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions.NotImplementedException
{
    public class WhenThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        public WhenThenThrowsNotImplementedException()
        {
            Assert.Throws(typeof(InconclusiveException), () => Sut.Execute(thenShouldThrow: true));
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
        public void ThenIsReportedAsNotImplemeneted()
        {
            Assert.Equal(Sut.ThenStep.Result, StepExecutionResult.NotImplemented);
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