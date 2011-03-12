using System;

namespace Bddify.Tests.BddifySpecs.Exceptions.OtherExceptions
{
    public class OtherExceptionBase
    {
        protected readonly ExceptionThrowingTest<Exception> Sut;

        public OtherExceptionBase()
        {
            Sut = new ExceptionThrowingTest<Exception>();
        }
    }
}