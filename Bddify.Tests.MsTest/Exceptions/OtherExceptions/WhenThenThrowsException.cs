using System;
using Bddify.Core;
using Bddify.Tests.Exceptions.OtherExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bddify.Tests.MsTest.Exceptions.OtherExceptions
{
    [TestClass]
    public class WhenThenThrowsException : OtherExceptionBase
    {
        [TestInitialize]
        public void SetupContext()
        {
            try
            {
                Sut.Execute(thenShouldThrow: true);
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail("Should not have reached here");
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
        public void ThenIsReportedAsFailed()
        {
            Assert.AreEqual(Sut.ThenStep.Result, StepExecutionResult.Failed);
        }

        [TestMethod]
        public void ThenScenarioResultReturnsFailed()
        {
            Assert.AreEqual(Sut.Scenario.Result, StepExecutionResult.Failed);
        }

        [TestMethod]
        public void ThenStoryResultReturnsFailed()
        {
            Assert.AreEqual(Sut.Scenario.Result, StepExecutionResult.Failed);
        }
    }
}