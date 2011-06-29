using System;
using Bddify.Core;
using System.Linq;
using Bddify.Scanners.StepScanners.Fluent;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions
{
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

        public void Execute(ThrowingMethods methodToThrow, bool useFluentScanner)
        {
            SetThrowingStep(methodToThrow);

            Bddifier bddify;

            if (useFluentScanner)
                bddify = FluentScannerBddifier();
            else
                bddify = ReflectingScannersBddifier();

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

        private Bddifier FluentScannerBddifier()
        {
            return this.Scan()
                        .Given(s => s.Given())
                        .When(s => s.When())
                        .Then(s => s.Then())
                        .TearDownWith(s => s.TearDown())
                        .LazyBddify();
        }

        private Bddifier ReflectingScannersBddifier()
        {
            return this.LazyBddify();
        }

        private void SetThrowingStep(ThrowingMethods methodToThrow)
        {
            _givenShouldThrow = false;
            _whenShouldThrow = false;
            _thenShouldThrow = false;

            switch (methodToThrow)
            {
                case ThrowingMethods.Given:
                    _givenShouldThrow = true;
                    break;

                case ThrowingMethods.When:
                    _whenShouldThrow = true;
                    break;

                case ThrowingMethods.Then:
                    _thenShouldThrow = true;
                    break;
            }
        }

        private ExecutionStep GetStep(string readableName)
        {
            return _scenario.Steps.First(s => s.ReadableMethodName == readableName);
        }

        ExecutionStep GivenStep
        {
            get
            {
                return GetStep("Given");
            }
        }

        ExecutionStep WhenStep
        {
            get {
                return GetStep("When");
            }
        }

        ExecutionStep ThenStep
        {
            get
            {
                return GetStep("Then");
            }
        }

        ExecutionStep TearDownStep
        {
            get
            {
                return GetStep("Tear down");
            }
        }

        Scenario Scenario
        {
            get
            {
                return _scenario;
            }
        }

        Core.Story Story { get; set; }
 
        public void AssertTearDownMethodIsExecuted()
        {
            Assert.That(TearDownStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        public void AssertGivenStepResult(StepExecutionResult result)
        {
            Assert.That(GivenStep.Result, Is.EqualTo(result));
        }

        public void AssertWhenStepResult(StepExecutionResult result)
        {
            Assert.That(WhenStep.Result, Is.EqualTo(result));
        }

        public void AssertThenStepResult(StepExecutionResult result)
        {
            Assert.That(ThenStep.Result, Is.EqualTo(result));
        }

        public void AssertScenarioResult(StepExecutionResult result)
        {
            Assert.That(Scenario.Result, Is.EqualTo(result));
        }

        public void AssertStoryResult(StepExecutionResult result)
        {
            Assert.That(Story.Result, Is.EqualTo(result));
        }
    }
}