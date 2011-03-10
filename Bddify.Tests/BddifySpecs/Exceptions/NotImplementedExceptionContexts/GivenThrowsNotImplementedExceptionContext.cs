using System;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class GivenThrowsNotImplementedExceptionContext : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(givenShouldThrow:true));
        }

        [Test]
        public void GivenIsReportedAsNotImplemented()
        {
            Reporter.Received().ReportNotImplemented(GetMethodInfo(Sut.Given), Arg.Any<NotImplementedException>());
        }

        [Test]
        public void WhenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.When));
        }

        [Test]
        public void ThenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Then));
        }
    }
}