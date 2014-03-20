using System.Linq;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Exceptions
{
    [TestFixture]
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

        [SetUp]
        public void InconclusiveExceptionSetup()
        {
            _engine = new InconclusiveTestClass().LazyBDDfy();
            Assert.Throws<InconclusiveException>(() => _engine.Run());
            _scenario = _engine.Story.Scenarios.First();
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