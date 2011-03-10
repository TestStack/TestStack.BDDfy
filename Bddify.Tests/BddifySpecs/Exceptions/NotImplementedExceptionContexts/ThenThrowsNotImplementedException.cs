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
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.Given));
        }

        [Test]
        public void WhenIsReportedAsSuccessful()
        {
            Reporter.Received().ReportSuccess(GetMethodInfo(Sut.When));
        }

        [Test]
        public void ThenIsReportedAsNotImplemeneted()
        {
            Reporter.Received().ReportNotImplemented(GetMethodInfo(Sut.Then));
        }
    }
}