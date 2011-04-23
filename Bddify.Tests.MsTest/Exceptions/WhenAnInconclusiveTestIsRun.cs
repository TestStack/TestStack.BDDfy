using System.Linq;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bddify.Tests.MsTest.Exceptions
{
    [TestClass]
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


        [TestInitialize]
        public void InconclusiveExceptionSetup()
        {
            var testClass = new InconclusiveTestClass();
            _bddifier = testClass.LazyBddify();

            try
            {
                _bddifier.Run();
            }
            catch (AssertInconclusiveException)
            {
                _scenario = _bddifier.Story.Scenarios.First();
                return;
            }

            Assert.Fail("Should not have reached here");
        }

        [TestMethod]
        public void ResultIsInconclusive()
        {
            Assert.AreEqual(_scenario.Result, StepExecutionResult.Inconclusive);
        }

        [TestMethod]
        public void ThenIsFlaggedAsInconclusive()
        {
            Assert.AreEqual(ThenStep.Result, StepExecutionResult.Inconclusive);
        }

        [TestMethod]
        public void ThenHasAnInconclusiveExceptionOnIt()
        {
            Assert.AreEqual(ThenStep.Exception.GetType(), typeof(AssertInconclusiveException));
        }

        [TestMethod]
        public void GivenIsFlaggedAsSuccessful()
        {
            Assert.AreEqual(GivenStep.Result, StepExecutionResult.Passed);
        }

        [TestMethod]
        public void WhenIsFlaggedAsSuccessful()
        {
            Assert.AreEqual(WhenStep.Result, StepExecutionResult.Passed);
        }

        [TestMethod]
        public void ThenScenarioResultReturnsInconclusive()
        {
            Assert.AreEqual(_scenario.Result, StepExecutionResult.Inconclusive);
        }

        [TestMethod]
        public void ThenStoryResultReturnsInconclusive()
        {
            Assert.AreEqual(_scenario.Result, StepExecutionResult.Inconclusive);
        }
    }
}