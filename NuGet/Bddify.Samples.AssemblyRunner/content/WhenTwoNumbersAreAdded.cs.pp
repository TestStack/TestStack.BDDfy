using System;
using Bddify.Core;

namespace $rootnamespace$.Bddify.Samples.AssemblyRunner
{
	[RunScenarioWithArgs(1, 2, 3)]
	[RunScenarioWithArgs(4, 5, 9)]
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
            if(_input1 + _input2 != _expectedResult)
                throw new Exception("Dude, addition aint working");
		}
	}
}