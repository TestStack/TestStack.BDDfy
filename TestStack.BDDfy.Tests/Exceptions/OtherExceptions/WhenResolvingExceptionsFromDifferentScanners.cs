using System;
using System.Xml;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenResolvingExceptionsFromDifferentScanners
    {
        private ExceptionThrowingTest<OuterException> _sut;

        /// <summary>
        /// Test to resolve issue where fluent scanner was returning the inner exception rather than the outer one.
        /// Issue 17: https://github.com/TestStack/TestStack.BDDfy/issues/17
        /// </summary>
        [Fact]
        public void FluentScannerShouldReturnOriginalExceptionAndNotInnerException()
        {
            _sut = new ExceptionThrowingTest<OuterException>();
            Should.Throw<OuterException>(() => _sut.Execute(ThrowingMethods.Given, true));
        }

        [Fact]
        public void ReflectiveScannerShouldReturnOriginalExceptionAndNotTargetInvocationException()
        {
            _sut = new ExceptionThrowingTest<OuterException>();
            Should.Throw<OuterException>(() => _sut.Execute(ThrowingMethods.Given, false));
        }

        private class OuterException : Exception
        {
            public OuterException()
                : base("outer", new XmlException("inner exception"))
            {
            }
        }
    }
}
