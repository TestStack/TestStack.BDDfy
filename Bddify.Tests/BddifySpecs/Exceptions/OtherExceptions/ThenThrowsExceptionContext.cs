using System;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class ThenThrowsExceptionContext : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(thenShouldThrow : true));
        }

        [Test]
        public void GivenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Given));
        }

        [Test]
        public void WhenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.When));
        }

        [Test]
        public void ThenIsReportedAsFailed()
        {
            Reporter.Received().ReportException(GetMethodInfo(Sut.Then), Arg.Any<Exception>());
        }
    }
}