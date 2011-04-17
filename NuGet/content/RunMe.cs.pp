using System;
using Bddify.Core;
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

	[Story(
    AsA = "As a second grader",
    IWant = "I want a calculator with four main functions",
    SoThat = "So I do not have to learn to calculate!!")]
    [TestClass]
	public class CalculatorCanDoFourMainMathFunctions
    {
        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }

	[RunScenarioWithArgs(1, 2, 3)]
	[RunScenarioWithArgs(4, 5, 8)] // failing scenario
    [WithStory(typeof(CalculatorCanDoFourMainMathFunctions))]
	public class WhenTwoNumbersAreAdded
	{
		private int _expectedResult;
	    private int _input1;
	    private int _input2;

	    void RunScenarioWithArgs(int input1, int input2, int expectedResult)
		{
			_input1 = input1;
			_input2 = input2;
			_expectedResult = expectedResult;
		}

		void ThenTheResultIsCorrect()
		{
            Assert.AreEqual(_input1 + _input2, _expectedResult);
		}
	}
	
    [WithStory(typeof(CalculatorCanDoFourMainMathFunctions))]
    public class WhenTwoNumbersAreDevided
    {
        void ThenTheResultIsCorrect()
        {
            Assert.Inconclusive();
        }
    }

    [RunScenarioWithArgs(1, 2, 2)]
    [RunScenarioWithArgs(4, 5, 20)]
    [WithStory(typeof(CalculatorCanDoFourMainMathFunctions))]
    public class WhenTwoNumbersAreMultiplied
    {
        private int _expectedResult;
        private int _input1;
        private int _input2;

        void RunScenarioWithArgs(int input1, int input2, int expectedResult)
        {
            _input1 = input1;
            _input2 = input2;
            _expectedResult = expectedResult;
        }

        void ThenTheResultIsCorrect()
        {
            Assert.AreEqual(_input1 * _input2, _expectedResult);
        }
    }

    [WithStory(typeof(CalculatorCanDoFourMainMathFunctions))]
    public class WhenTwoNumbersAreSubtracted
    {
        [RunStepWithArgs(5, 3, 2)]
        [RunStepWithArgs(1, 8, -7)]
        void ThenTheResultShouldBeCorrect(int input1, int input2, int expectedResult)
        {
            Assert.AreEqual(input1 - input2, expectedResult);
        }
    }
}