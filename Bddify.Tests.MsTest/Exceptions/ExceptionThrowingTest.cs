using System;
using System.Reflection;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using System.Linq;

namespace Bddify.Tests.MsTest.Exceptions
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

        private ExecutionStep GetStep(MethodInfo stepMethod)
        {
            return _scenario.Steps.Single(s => s.ReadableMethodName == NetToString.Convert(stepMethod.Name));
        }

        public ExecutionStep GivenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(Given));
            }
        }

        public ExecutionStep WhenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(When));
            }
        }

        public ExecutionStep ThenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(Then));
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