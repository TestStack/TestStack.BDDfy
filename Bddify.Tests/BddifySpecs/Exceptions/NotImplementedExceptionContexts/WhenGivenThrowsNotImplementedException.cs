using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class WhenGivenThrowsNotImplementedException : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(givenShouldThrow:true));
        }

        [Test]
        public void GivenIsReportedAsNotImplemented()
        {
            Assert.That(Sut.GivenStep.Result, Is.EqualTo(StepExecutionResult.NotImplemented));
        }

        [Test]
        public void WhenIsNotExecuted()
        {
            Assert.That(Sut.WhenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
        }

        [Test]
        public void ThenIsNotExecuted()
        {
            Assert.That(Sut.ThenStep.Result, Is.EqualTo(StepExecutionResult.NotExecuted));
        }
    }
}