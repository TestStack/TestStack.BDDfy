using System.Linq;
using Bddify.Core;
using Bddify.Processors;
using Xunit;

namespace Bddify.Tests.MsTest.Exceptions
{
    public class WhenAnInconclusiveTestIsRun
    {
        public class InconclusiveTestClass
        {
            public void GivenA_classUnderTest()
            {
            }

            public void WhenInconclusiveExceptionIsThrownInOneOfTheMethods()
            {
            }

            public void ThenTheContextIsFlaggedAsInconclusive()
            {
                throw new InconclusiveException();
            }
        }

        readonly Bddifier _bddifier;
        Scenario _scenario;


        public WhenAnInconclusiveTestIsRun()
        {
            var testClass = new InconclusiveTestClass();
            _bddifier = testClass.LazyBddify();
            Assert.Throws(typeof(InconclusiveException), () => _bddifier.Run());
            _scenario = _bddifier.Story.Scenarios.First();
        }

        InconclusiveTestClass TestClass
        {
            get
            {
                return (InconclusiveTestClass)_scenario.TestObject;
            }
        }

        ExecutionStep GivenStep
        {
            get
            {
                return _scenario.Steps.First(s => s.Method == Helpers.GetMethodInfo(TestClass.GivenA_classUnderTest));
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

        [Fact]
        public void ResultIsInconclusive()
        {
            Assert.Equal(_scenario.Result, StepExecutionResult.Inconclusive);
        }

        [Fact]
        public void ThenIsFlaggedAsInconclusive()
        {
            Assert.Equal(ThenStep.Result, StepExecutionResult.Inconclusive);
        }

        [Fact]
        public void ThenHasAnInconclusiveExceptionOnIt()
        {
            Assert.Equal(ThenStep.Exception.GetType(), typeof(InconclusiveException));
        }

        [Fact]
        public void GivenIsFlaggedAsSuccessful()
        {
            Assert.Equal(GivenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void WhenIsFlaggedAsSuccessful()
        {
            Assert.Equal(WhenStep.Result, StepExecutionResult.Passed);
        }

        [Fact]
        public void ThenScenarioResultReturnsInconclusive()
        {
            Assert.Equal(_scenario.Result, StepExecutionResult.Inconclusive);
        }

        [Fact]
        public void ThenStoryResultReturnsInconclusive()
        {
            Assert.Equal(_scenario.Result, StepExecutionResult.Inconclusive);
        }
    }
}