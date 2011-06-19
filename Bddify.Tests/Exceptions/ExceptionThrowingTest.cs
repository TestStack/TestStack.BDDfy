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
    public enum ThrowingMethod
    {
        None,
        Given,
        When,
        Then
    }

    public class ExceptionThrowingTest<T> where T : Exception, new()
    {
        private static bool _givenShouldThrow;
        private static bool _whenShouldThrow;
        private static bool _thenShouldThrow;
        Scenario _scenario;

        void Given()
        {
            if (_givenShouldThrow)
                throw new T();
        }

        void When()
        {
            if(_whenShouldThrow)
                throw new T();
        }

        void Then()
        {
            if (_thenShouldThrow)
                throw new T();
        }

        void TearDown()
        {
        }

        public void Execute(ThrowingMethod methodToThrow)
        {
            _givenShouldThrow = false;
            _whenShouldThrow = false;
            _thenShouldThrow = false;

            switch (methodToThrow)
            {
                case ThrowingMethod.Given:
                    _givenShouldThrow = true;
                    break;

                case ThrowingMethod.When:
                    _whenShouldThrow = true;
                    break;

                case ThrowingMethod.Then:
                    _thenShouldThrow = true;
                    break;
            }

            var bddify = new Bddifier(
                typeof(ExceptionThrowingTest<T>),
                new DefaultScanner(new ScanForScenarios(new[] {new DefaultMethodNameStepScanner()})),
                new IProcessor[]
                    {
                        new TestRunner(), 
                        new ConsoleReporter(),
                        new ExceptionProcessor(),
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

        private ExecutionStep GetStep(string readableName)
        {
            return _scenario.Steps.First(s => s.ReadableMethodName == readableName);
        }

        public ExecutionStep GivenStep
        {
            get
            {
                return GetStep("Given");
            }
        }

        public ExecutionStep WhenStep
        {
            get {
                return GetStep("When");
            }
        }

        public ExecutionStep ThenStep
        {
            get
            {
                return GetStep("Then");
            }
        }

        public ExecutionStep TearDownStep
        {
            get
            {
                return GetStep("Tear down");
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