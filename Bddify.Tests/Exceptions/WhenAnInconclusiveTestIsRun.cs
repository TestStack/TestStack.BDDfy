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
        }

        Bddifier _bddifier;
        private Scenario _scenario;

        InconclusiveTestClass TestClass
        {
            get
            {
                return (InconclusiveTestClass)_scenario.Object;
            }
        }

        ExecutionStep GivenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(TestClass.GivenAClassUnderTest));
            }
        }

        ExecutionStep WhenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(TestClass.WhenInconclusiveExceptionIsThrownInOneOfTheMethods));
            }
        }

        ExecutionStep ThenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(TestClass.ThenTheContextIsFlaggedAsInconclusive));
            }
        }


        [SetUp]
        public void InconclusiveExceptionSetup()
        {
            var testClass = new InconclusiveTestClass();
            _bddifier = testClass.LazyBddify();
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
        public void ThenScenarioResultReturnsInconclusive()
        {
            Assert.That(_scenario.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }

        [Test]
        public void ThenStoryResultReturnsInconclusive()
        {
            Assert.That(_scenario.Result, Is.EqualTo(StepExecutionResult.Inconclusive));
        }
    }
}