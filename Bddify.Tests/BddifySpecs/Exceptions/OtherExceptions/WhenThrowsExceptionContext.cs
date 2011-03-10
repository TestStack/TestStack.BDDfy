using System;
using NSubstitute;
using NUnit.Framework;

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
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Given));
        }

        [Test]
        public void WhenIsReportedAsFailed()
        {
            Reporter.Received().ReportException(GetMethodInfo(Sut.When), Arg.Any<Exception>());
        }

        [Test]
        public void ThenIsNotExecuted()
        {
            Reporter.DidNotReceive().ReportSuccess(GetMethodInfo(Sut.Then));
        }
    }
}