using System;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class GivenThrowsExceptionContext : OtherExceptionBase
    {
        [SetUp]
        public void SetupContext()
        {
            Assert.Throws<Exception>(() => Sut.Execute(givenShouldThrow:true));
        }

        [Test]
        public void GivenShouldBeReportedAsFailed()
        {
            Reporter.Received().ReportException(GetMethodInfo(Sut.Given), Arg.Any<Exception>());
        }

        [Test]
        public void WhenShouldNotBeExecuted()
        {
            Reporter.DidNotReceive().ReportSuccess(GetMethodInfo(Sut.When));
        }

        [Test]
        public void ThenShouldNotBeExecuted()
        {
            Reporter.DidNotReceive().ReportSuccess(GetMethodInfo(Sut.Then));
        }

    }
}