using System;
using NSubstitute;

namespace Bddify.Tests.BddifySpecs.Exceptions.NotImplementedExceptionContexts
{
    public class NotImplementedExceptionBase : HandlingExceptionBase
    {
        protected readonly IBddifyReporter Reporter;
        protected readonly ExceptionThrowingTest<NotImplementedException> Sut;

        public NotImplementedExceptionBase()
        {
            Reporter = Substitute.For<IBddifyReporter>();
            Sut = new ExceptionThrowingTest<NotImplementedException>(Reporter);
        }
    }
}