// This is a class that represents one Scenario type. This scenario is decorated with RunScenarioWithArgs attribute.
// There is also a method called RunScenarioWithArgs that has the same number of arguments as provided on the RunScenarioWithArgs attributes.
// Bddify creates three scenarios from this type, that one per RunScenarioWithArgs instance. Two of these scenarios will succeed; 
// but one of them will fail.
// Again it is worth mentioning that this is not to be considered as a "how to do BDD" sample; this is merely to show you how bddify deals with
// RunScenarioWithArgs and failing scenarios.

using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.NUnit
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
            Assert.That(_actualResult, Is.EqualTo(_expectedResult));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}