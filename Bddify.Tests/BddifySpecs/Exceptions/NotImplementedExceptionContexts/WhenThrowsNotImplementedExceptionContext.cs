using System;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class WhenThrowsNotImplementedExceptionContext : NotImplementedExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<InconclusiveException>(() => Sut.Execute(whenShouldThrow:true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Given));
        }

        [Test]
        public void WhenIsReportedAsNotImplemented()
        {
            Reporter.Received().ReportNotImplemented(GetMethodInfo(Sut.When), Arg.Any<NotImplementedException>());
        }

        [Test]
        public void ThenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Then));
        }
    }
}