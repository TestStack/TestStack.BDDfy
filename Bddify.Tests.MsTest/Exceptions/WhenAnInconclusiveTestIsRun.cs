using System.Linq;
using System.Reflection;
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
                return (InconclusiveTestClass)_scenario.TestObject;
            }
        }

        private ExecutionStep GetStep(MethodInfo stepMethod)
        {
            return _scenario.Steps.Single(s => s.ReadableMethodName == NetToString.Convert(stepMethod.Name));
        }

        ExecutionStep GivenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(TestClass.GivenAClassUnderTest));
            }
        }

        ExecutionStep WhenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(TestClass.WhenInconclusiveExceptionIsThrownInOneOfTheMethods));
            }
        }

        ExecutionStep ThenStep
        {
            get
            {
                return GetStep(Helpers.GetMethodInfo(TestClass.ThenTheContextIsFlaggedAsInconclusive));
            }
        }


        [TestInitialize]
        public void InconclusiveExceptionSetup()
        {
            _bddifier = typeof(InconclusiveTestClass).LazyBddify();

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