using System.Linq;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Exceptions
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
                Assert.Inconclusive();
            }

            public void TearDownThis()
            {
                
            }
        }

        Bddifier _bddifier;
        private Scenario _scenario;

        ExecutionStep GivenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.ReadableMethodName == "Given a class under test");
            }
        }

        ExecutionStep WhenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.ReadableMethodName == "When inconclusive exception is thrown in one of the methods");
            }
        }

        ExecutionStep ThenStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.ReadableMethodName == "Then the context is flagged as inconclusive");
            }
        }

        ExecutionStep DisposeStep
        {
            get
            {
                return _scenario.Steps.Single(s => s.ReadableMethodName == "Tear down this");
            }
        }

        [SetUp]
        public void InconclusiveExceptionSetup()
        {
            _bddifier = typeof(InconclusiveTestClass).LazyBddify();
            Assert.Throws<InconclusiveException>(() => _bddifier.Run());
            _scenario = _bddifier.Story.Scenarios.First();
        }

        [Test]
        public void ResultIsInconclusive()
        {
            Assert.That(_scenario.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }

        [Test]
        public void ThenIsFlaggedAsInconclusive()
        {
            Assert.That(ThenStep.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }

        [Test]
        public void ThenHasAnInconclusiveExceptionOnIt()
        {
            Assert.That(ThenStep.Exception, Is.AssignableFrom(typeof(InconclusiveException)));
        }

        [Test]
        public void GivenIsFlaggedAsSuccessful()
        {
            Assert.That(GivenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void WhenIsFlaggedAsSuccessful()
        {
            Assert.That(WhenStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }

        [Test]
        public void ScenarioResultReturnsInconclusive()
        {
            Assert.That(_scenario.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }

        [Test]
        public void StoryResultReturnsInconclusive()
        {
            Assert.That(_scenario.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }

        [Test]
        public void TearDownMethodIsExecuted()
        {
            Assert.That(DisposeStep.Result, Is.EqualTo(StepExecutionResult.Passed));
        }
    }
}