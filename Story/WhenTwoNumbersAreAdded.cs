using Bddify.Core;
using NUnit.Framework;

namespace StoryDemo
{
	[RunScenarioWithArgs(1, 2, 3)]
	[RunScenarioWithArgs(4, 5, 9)]
    [WithStory(typeof(CalculatorStory))]
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
            Assert.That(_input1 + _input2, Is.EqualTo(_expectedResult));
		}
	}
}