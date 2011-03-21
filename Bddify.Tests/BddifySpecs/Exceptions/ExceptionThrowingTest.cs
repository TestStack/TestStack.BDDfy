using System;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using NSubstitute;
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
        Scenario _scenario;

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

            _bddify = new Bddifier(
                this, 
                new GwtScanner(), 
                new IProcessor[]
                    {
                        new TestRunner<InconclusiveException>(), 
                        new ConsoleReporter(),
                        new ExceptionHandler(Assert.Inconclusive)
                    });
            try
            {
                _bddify.Run();
            }
            finally 
            {
                _scenario = _bddify.Scenarios.First();
            }
        }

        public ExecutionStep GivenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(Given));
            }
        }

        public ExecutionStep WhenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(When));
            }
        }

        public ExecutionStep ThenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(Then));
            }
        }
    }
}