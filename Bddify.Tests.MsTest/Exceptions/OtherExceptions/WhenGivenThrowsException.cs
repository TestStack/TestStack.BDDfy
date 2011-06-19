using System;
using Bddify.Core;
using Bddify.Tests.Exceptions;
using Bddify.Tests.Exceptions.OtherExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bddify.Tests.MsTest.Exceptions.OtherExceptions
{
    [TestClass]
    public class WhenGivenThrowsException : OtherExceptionBase
    {
        [TestInitialize]
        public void SetupContext()
        {
            try
            {
                Sut.Execute(ThrowingMethod.Given);
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail("Should not have reached here");
        }

        [TestMethod]
        public void GivenShouldBeReportedAsFailed()
        {
            Assert.AreEqual(Sut.GivenStep.Result, StepExecutionResult.Failed);
        }

        [TestMethod]
        public void WhenShouldNotBeExecuted()
        {
            Assert.AreEqual(Sut.WhenStep.Result, StepExecutionResult.NotExecuted);
        }

        [TestMethod]
        public void ThenShouldNotBeExecuted()
        {
            Assert.AreEqual(Sut.ThenStep.Result, StepExecutionResult.NotExecuted);
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