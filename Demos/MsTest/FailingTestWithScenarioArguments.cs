using Bddify.Core;
using Bddify.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.MsTest
{
    [RunScenarioWithArgs(1, 2, 3)]
    [RunScenarioWithArgs(-1, 5, 4)]
    [RunScenarioWithArgs(3, 7, 9)] // failing test
    public class FailingTestWithScenarioArguments
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
            Assert.AreEqual(_actualResult, _expectedResult);
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify<MethodNameScanner>();
        }
    }
}