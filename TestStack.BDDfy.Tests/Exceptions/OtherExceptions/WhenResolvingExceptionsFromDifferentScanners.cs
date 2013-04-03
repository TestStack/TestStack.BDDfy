using System;
using System.Xml;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions.OtherExceptions
{
    public class WhenResolvingExceptionsFromDifferentScanners
    {
        private ExceptionThrowingTest<OuterException> _sut;

        /// <summary>
        /// Test to resolve issue where fluent scanner was returning the inner exception rather than the outer one.
        /// Issue 17: https://github.com/TestStack/TestStack.BDDfy/issues/17
        /// </summary>
        [Test]
        public void FluentScannerShouldReturnOriginalExceptionAndNotInnerException()
        {
            _sut = new ExceptionThrowingTest<OuterException>();
            Assert.Throws<OuterException>(() => _sut.Execute(ThrowingMethods.Given, true));
        }

        [Test]
        public void ReflectiveScannerShouldReturnOriginalExceptionAndNotTargetInvocationException()
        {
            _sut = new ExceptionThrowingTest<OuterException>();
            Assert.Throws<OuterException>(() => _sut.Execute(ThrowingMethods.Given, false));
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
