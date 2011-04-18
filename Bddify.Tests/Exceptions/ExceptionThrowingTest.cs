using System;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Exceptions
{
    public class ExceptionThrowingTest<T> where T : Exception, new()
    {
        private static bool _givenShouldThrow;
        private static bool _whenShouldThrow;
        private static bool _thenShouldThrow;
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

            var bddify = new Bddifier(
                this,
                new DefaultScanner(new ScanForScenarios(new DefaultScanForStepsByMethodName())),
                new IProcessor[]
                    {
                        new TestRunner(), 
                        new ConsoleReporter(),
                        new ExceptionProcessor(Assert.Inconclusive),
                    });
            try
            {
                bddify.Run();
            }
            finally 
            {
                Story = bddify.Story;
                _scenario = bddify.Story.Scenarios.First();
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

        public Scenario Scenario
        {
            get
            {
                return _scenario;
            }
        }

        public Core.Story Story { get; private set; }
    }
}