using System;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.BddifySpecs.Exceptions
{
    public class ExceptionThrowingTest<T> where T : Exception, new()
    {
        private bool _givenShouldThrow;
        private bool _whenShouldThrow;
        private bool _thenShouldThrow;
        private Bddifier _bddify;

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

            _bddify = new Bddifier(Assert.Inconclusive, new GwtScanner(), new TestRunner<InconclusiveException>(), new ConsoleReporter(), this);
            _bddify.Run();
        }

        public ExecutionStep GivenStep
        {
            get
            {
                return _bddify.Bddee.Steps.First(s => s.Method == Helpers.GetMethodInfo(Given));
            }
        }

        public ExecutionStep WhenStep
        {
            get
            {
                return _bddify.Bddee.Steps.First(s => s.Method == Helpers.GetMethodInfo(When));
            }
        }

        public ExecutionStep ThenStep
        {
            get
            {
                return _bddify.Bddee.Steps.First(s => s.Method == Helpers.GetMethodInfo(Then));
            }
        }
    }
}