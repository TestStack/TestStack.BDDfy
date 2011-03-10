using System;
using NSubstitute;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class OtherExceptionBase : HandlingExceptionBase
    {
        protected readonly IBddifyReporter Reporter;
        protected readonly ExceptionThrowingTest<Exception> Sut;

        public OtherExceptionBase()
        {
            Reporter = Substitute.For<IBddifyReporter>();
            Sut = new ExceptionThrowingTest<Exception>(Reporter);
        }
    }
}