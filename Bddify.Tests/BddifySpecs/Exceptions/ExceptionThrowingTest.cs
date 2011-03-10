using System;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Exceptions
{
    public class ExceptionThrowingTest<T> where T : Exception, new()
    {
        private readonly IBddifyReporter _reporter;
        private bool _givenShouldThrow;
        private bool _whenShouldThrow;
        private bool _thenShouldThrow;

        public ExceptionThrowingTest(IBddifyReporter reporter)
        {
            _reporter = reporter;
        }

        [Given]
        public void Given()
        {
            if (_givenShouldThrow)
                throw new T();
        }

        [When]
        public void When()
        {
            if(_whenShouldThrow)
                throw new T();
        }

        [Then]
        public void Then()
        {
            if (_thenShouldThrow)
                throw new T();
        }

        public void Execute(bool givenShouldThrow = false, bool whenShouldThrow = false, bool thenShouldThrow = false)
        {
            _givenShouldThrow = givenShouldThrow;
            _whenShouldThrow = whenShouldThrow;
            _thenShouldThrow = thenShouldThrow;

            var bddify = new Bddifier(_reporter, new Scanner(), new InconclusiveException(string.Empty), this);
            bddify.Run();
        }
    }
}