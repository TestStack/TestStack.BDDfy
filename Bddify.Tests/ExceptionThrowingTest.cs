using System;
using System.Reflection;
using System.Linq;

namespace Bddify.Tests
{
    public class ExceptionThrowingTest
    {
        private readonly IBddifyReporter _reporter;
        private readonly bool _givenShouldThrow;
        private readonly bool _whenShouldThrow;
        private readonly bool _thenShouldThrow;
        private readonly Type _exceptionTypeToBeThrown;

        public ExceptionThrowingTest(
            IBddifyReporter reporter,
            Type exceptionTypeToBeThrown,
            bool givenShouldThrow = false, 
            bool whenShouldThrow = false, 
            bool thenShouldThrow = false)
        {
            _reporter = reporter;
            _givenShouldThrow = givenShouldThrow;
            _whenShouldThrow = whenShouldThrow;
            _thenShouldThrow = thenShouldThrow;
            _exceptionTypeToBeThrown = exceptionTypeToBeThrown;
        }

        Exception TheException
        {
            get
            {
                return (Exception)Activator.CreateInstance(_exceptionTypeToBeThrown);
            }
        }

        [Given]
        public void Given()
        {
            if (_givenShouldThrow)
                throw TheException;
        }

        [When]
        public void When()
        {
            if(_whenShouldThrow)
                throw TheException;
        }

        [Then]
        public void Then()
        {
            if (_thenShouldThrow)
                throw TheException;
        }

        public void Execute()
        {
            var bddify = new Bddifier(_reporter, this);
            bddify.Run();
        }
    }
}