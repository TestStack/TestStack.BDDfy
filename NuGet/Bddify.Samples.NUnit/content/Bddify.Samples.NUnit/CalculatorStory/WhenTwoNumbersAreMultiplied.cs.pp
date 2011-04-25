using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.CalculatorStory
{
    [RunScenarioWithArgs(1, 2, 2)]
    [RunScenarioWithArgs(4, 5, 20)]
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
            Assert.That(_input1 * _input2, Is.EqualTo(_expectedResult));
        }
    }
}