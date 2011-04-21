using System;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$
{
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