using System;
using Bddify.Core;
using Bddify.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$
{
    [RunScenarioWithArgs(1, 2, 3)]
    [RunScenarioWithArgs(-1, 5, 4)]
	[TestClass]
    public class ScenarioWithArgs
    {
        private int _expectedResult;
        private int _input1;
        private int _input2;
        private int _actualResult;

        void RunScenarioWithArgs(int input1, int input2, int expectedResult)
        {
            _input1 = input1;
            _input2 = input2;
            _expectedResult = expectedResult;
        }

        void GivenTwoNumbers()
        {
        }

        void WhenTheNumbersAreAdded()
        {
            _actualResult = _input1 + _input2;
        }

        void ThenTheResultIsCorrect()
        {
            Assert.AreEqual(_actualResult, _expectedResult);
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }

    [TestClass]
    public class SomeIncompleteTest
    {
        void GivenThisTestOrOneOfTheClassesItCallsToIsIncomplete()
        {
        }

        void WhenTheTestIsRun()
        {
            throw new NotImplementedException();
        }

        void ThenItIsFlaggedAsIncomplete()
        {
            
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }

    [TestClass]
    public class SomeInconclusiveTest
    {
        void GivenThisTestThrowsInconclusiveException()
        {
        }

        void WhenTheTestIsRun()
        {
        }

        void ThenItIsFlaggedAsInconclusive()
        {
			Assert.Inconclusive();            
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }

	[TestClass]
    public class WhenTwoNumbersAreSubtracted
    {
        [RunStepWithArgs(5, 3, 2)]
        [RunStepWithArgs(1, 8, -7)]
        [RunStepWithArgs(2, 3, 0)] // this should fail
        void ThenTheResultShouldBeCorrect(int input1, int input2, int expectedResult)
        {
            if (input1 - input2 != expectedResult)
                throw new Exception("Dude, subtract aint working");
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }
}