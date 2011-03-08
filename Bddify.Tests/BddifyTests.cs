using System;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests
{
    [TestFixture]
    public class BddifyTests
    {
        readonly IBddifyReporter _reporter = Reporter;

        static IBddifyReporter Reporter
        {
            get
            {
                var reporter = Substitute.For<IBddifyReporter>();

                reporter.ReportException(Arg.Any<MethodInfo>(), Arg.Any<Exception>()).Returns(ExecutionResult.Failed);
                reporter.ReportNotImplemented(Arg.Any<MethodInfo>()).Returns(ExecutionResult.NotImplemented);
                reporter.ReportSuccess(Arg.Any<MethodInfo>()).Returns(ExecutionResult.Succeeded);
                return reporter;
            }
        }
    
        [Test]
        public void WhenGivenThrowsNotImplementedException_NotImplementedExceptionIsThrownAndGivenIsReportedAsNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(NotImplementedException), givenShouldThrow:true);
            Assert.Throws<InconclusiveException>(sut.Execute);
            _reporter.Received().ReportNotImplemented(GetMethodInfo(sut.Given));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.When));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Then));
        }

        [Test]
        public void WhenGivenThrows_TestFailsnAndGivenIsReportedAsFailedNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(Exception), givenShouldThrow:true);
            Assert.Throws<AssertionException>(sut.Execute);
            _reporter.Received().ReportException(GetMethodInfo(sut.Given), Arg.Any<Exception>());
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.When));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Then));
        }

        [Test]
        public void WhenWhenThrows_TestFailsnAndWhenIsReportedAsFailedNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(Exception), whenShouldThrow:true);
            Assert.Throws<AssertionException>(sut.Execute);
            _reporter.Received().ReportException(GetMethodInfo(sut.When), Arg.Any<Exception>());
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Given));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Then));
        }

        [Test]
        public void WhenThenThrows_TestFailsnAndThenIsReportedAsFailedNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(Exception), thenShouldThrow:true);
            Assert.Throws<AssertionException>(sut.Execute);
            _reporter.Received().ReportException(GetMethodInfo(sut.Then), Arg.Any<Exception>());
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Given));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.When));
        }

        [Test]
        public void WhenWhenThrowsNotImplementedException_NotImplementedExceptionIsThrownAndWhenIsReportedAsNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(NotImplementedException), whenShouldThrow:true);
            Assert.Throws<InconclusiveException>(sut.Execute);
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Given));
            _reporter.Received().ReportNotImplemented(GetMethodInfo(sut.When));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Then));
        }

        [Test]
        public void WhenThenThrowsNotImplementedException_NotImplementedExceptionIsThrownAndThenIsReportedAsNotImplemented()
        {
            var sut = new ExceptionThrowingTest(_reporter, typeof(NotImplementedException), thenShouldThrow:true);
            Assert.Throws<InconclusiveException>(sut.Execute);
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.Given));
            _reporter.Received().ReportSuccess(GetMethodInfo(sut.When));
            _reporter.Received().ReportNotImplemented(GetMethodInfo(sut.Then));
        }

        static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }
    }
}