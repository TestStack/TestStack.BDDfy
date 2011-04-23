using Bddify.Core;
using Bddify.Tests.Exceptions.NotImplementedException;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bddify.Tests.MsTest.Exceptions.NotImplementedException
{
    [TestClass]
    public class WhenGivenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [TestInitialize]
        public void SetupContext()
        {
            try
            {
                Sut.Execute(givenShouldThrow: true);
            }
            catch (AssertInconclusiveException)
            {
                return;
            }

            Assert.Fail("Should not have reached here");
        }

        [TestMethod]
        public void GivenIsReportedAsNotImplemented()
        {
            Assert.AreEqual(Sut.GivenStep.Result, StepExecutionResult.NotImplemented);
        }

        [TestMethod]
        public void WhenIsNotExecuted()
        {
            Assert.AreEqual(Sut.WhenStep.Result, StepExecutionResult.NotExecuted);
        }

        [TestMethod]
        public void ThenIsNotExecuted()
        {
            Assert.AreEqual(Sut.ThenStep.Result, StepExecutionResult.NotExecuted);
        }

        [TestMethod]
        public void ThenScenarioResultReturnsNoImplemented()
        {
            Assert.AreEqual(Sut.Scenario.Result, StepExecutionResult.NotImplemented);
        }

        [TestMethod]
        public void ThenStoryResultReturnsNoImplemented()
        {
            Assert.AreEqual(Sut.Story.Result, StepExecutionResult.NotImplemented);
        }
    }
}