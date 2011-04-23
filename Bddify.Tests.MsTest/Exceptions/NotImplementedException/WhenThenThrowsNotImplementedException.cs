using Bddify.Core;
using Bddify.Tests.Exceptions.NotImplementedException;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bddify.Tests.MsTest.Exceptions.NotImplementedException
{
    [TestClass]
    public class WhenThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [TestInitialize]
        public void SetupContext()
        {
            try
            {
                Sut.Execute(thenShouldThrow:true);
            }
            catch (AssertInconclusiveException)
            {
                return;
            }

            Assert.Fail("We should not get here");
        }

        [TestMethod]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.AreEqual(Sut.GivenStep.Result, StepExecutionResult.Passed);
        }

        [TestMethod]
        public void WhenIsReportedAsSuccessful()
        {
            Assert.AreEqual(Sut.WhenStep.Result, StepExecutionResult.Passed);
        }

        [TestMethod]
        public void ThenIsReportedAsNotImplemeneted()
        {
            Assert.AreEqual(Sut.ThenStep.Result, StepExecutionResult.NotImplemented);
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