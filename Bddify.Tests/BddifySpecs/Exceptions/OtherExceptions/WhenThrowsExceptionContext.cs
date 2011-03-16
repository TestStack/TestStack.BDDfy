using System;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class WhenThrowsExceptionContext : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(whenShouldThrow : true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void WhenIsReportedAsFailed()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.Failed));
        }

        [Test]
        public void ThenIsNotExecuted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
        }
    }
}