using System;
using System.Linq;
using Shouldly;

namespace TestStack.BDDfy.Tests.Exceptions
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

            Engine engine = useFluentScanner ? FluentScannerBddifier() : ReflectingScannersBddifier();

            try
            {
                engine.Run();
            }
            finally
            {
                Story = engine.Story;
                _scenario = engine.Story.Scenarios.First();
            }
        }

        private Engine FluentScannerBddifier()
        {
            return this.Given(s => s.Given())
                        .When(s => s.When())
                        .Then(s => s.Then())
                        .TearDownWith(s => s.TearDown())
                        .LazyBDDfy();
        }

        private Engine ReflectingScannersBddifier()
        {
            return this.LazyBDDfy();
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

        private Step GetStep(string stepTitle)
        {
            return _scenario.Steps.First(s => s.Title == stepTitle);
        }

        Step GivenStep
        {
            get
            {
                return GetStep("Given");
            }
        }

        Step WhenStep
        {
            get {
                return GetStep("When");
            }
        }

        Step ThenStep
        {
            get
            {
                return GetStep("Then");
            }
        }

        Step TearDownStep
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

        Story Story { get; set; }
 
        public void AssertTearDownMethodIsExecuted()
        {
            TearDownStep.Result.ShouldBe(Result.Passed);
        }

        public void AssertGivenStepResult(Result result)
        {
            GivenStep.Result.ShouldBe(result);
        }

        public void AssertWhenStepResult(Result result)
        {
            WhenStep.Result.ShouldBe(result);
        }

        public void AssertThenStepResult(Result result)
        {
            ThenStep.Result.ShouldBe(result);
        }

        public void AssertScenarioResult(Result result)
        {
            Scenario.Result.ShouldBe(result);
        }

        public void AssertStoryResult(Result result)
        {
            Story.Result.ShouldBe(result);
        }
    }
}