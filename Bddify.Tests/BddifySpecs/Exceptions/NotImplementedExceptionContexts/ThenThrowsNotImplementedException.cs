using System;
using Bddify.Core;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class ThenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(thenShouldThrow:true));
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
        public void ThenIsReportedAsNotImplemeneted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
        }
    }
}