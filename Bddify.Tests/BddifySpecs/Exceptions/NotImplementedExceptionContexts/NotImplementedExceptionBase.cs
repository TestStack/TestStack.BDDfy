using System;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class NotImplementedExceptionBase
    {
        protected readonly ExceptionThrowingTest<NotImplementedException> Sut;

        public NotImplementedExceptionBase()
        {
            Sut = new ExceptionThrowingTest<NotImplementedException>();
        }
    }
}