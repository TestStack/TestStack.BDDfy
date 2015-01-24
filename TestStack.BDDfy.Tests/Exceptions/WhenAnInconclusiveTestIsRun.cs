using System.Linq;
using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions
{
    public class WhenAnInconclusiveTestIsRun
    {
        public class InconclusiveTestClass
        {
            public void GivenAClassUnderTest()
            {
            }

            public void WhenInconclusiveExceptionIsThrownInOneOfTheMethods()
            {
            }

            public void ThenTheContextIsFlaggedAsInconclusive()
            {
                throw new InconclusiveException();
            }

            public void TearDownThis()
            {
                
            }
        }

        Engine _engine;
        private Scenario _scenario;

        Step GivenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.Title == "Given a class under test");
            }
        }

        Step WhenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.Title == "When inconclusive exception is thrown in one of the methods");
            }
        }

        Step ThenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.Title == "Then the context is flagged as inconclusive");
            }
        }

        Step DisposeStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.Title == "Tear down this");
            }
        }

        public WhenAnInconclusiveTestIsRun()
        {
            _engine = new InconclusiveTestClass().LazyBDDfy();
            Should.Throw<InconclusiveException>(() => _engine.Run());
            _scenario = _engine.Story.Scenarios.First();
        }

        [Fact]
        public void ResultIsInconclusive()
        {
            _scenario.Result.ShouldBe(Result.Inconclusive);
        }

        [Fact]
        public void ThenIsFlaggedAsInconclusive()
        {
            ThenStep.Result.ShouldBe(Result.Inconclusive);
        }

        [Fact]
        public void ThenHasAnInconclusiveExceptionOnIt()
        {
            ThenStep.Exception.ShouldBeAssignableTo<InconclusiveException>();
        }

        [Fact]
        public void GivenIsFlaggedAsSuccessful()
        {
            GivenStep.Result.ShouldBe(Result.Passed);
        }

        [Fact]
        public void WhenIsFlaggedAsSuccessful()
        {
            WhenStep.Result.ShouldBe(Result.Passed);
        }

        [Fact]
        public void ScenarioResultReturnsInconclusive()
        {
            _scenario.Result.ShouldBe(Result.Inconclusive);
        }

        [Fact]
        public void StoryResultReturnsInconclusive()
        {
            _scenario.Result.ShouldBe(Result.Inconclusive);
        }

        [Fact]
        public void TearDownMethodIsExecuted()
        {
            DisposeStep.Result.ShouldBe(Result.Passed);
        }
    }
}