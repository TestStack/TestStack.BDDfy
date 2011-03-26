using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class WhenThenThrowsException : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(thenShouldThrow : true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void WhenIsReportedAsSuccessful()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void ThenIsReportedAsFailed()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.Failed));
        }
    }
}