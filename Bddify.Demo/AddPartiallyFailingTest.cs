using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Demo
{
    [RunScenarioWithArgs(1, 2, 3)]
    [RunScenarioWithArgs(-1, 5, 4)]
    public class TestWithScenarioArguments
    {
        private int _expectedResult;
        private int _input1;
        private int _input2;
        private int _actualResult;

        void RunScenarioWithArgs(int input1, int  input2, int expectedResult)
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
            Assert.That(_actualResult, Is.EqualTo(_expectedResult));
        }

        [Test]
        public void Execute()
        {
            this.Bddify<GwtScanner>();
        }
    }
}