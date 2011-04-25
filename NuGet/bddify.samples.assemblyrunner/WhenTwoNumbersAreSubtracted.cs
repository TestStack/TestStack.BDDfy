using System;
using Bddify.Core;

namespace AssemblyRunner
{
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
    }
}