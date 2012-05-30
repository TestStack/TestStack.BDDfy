using BDDfy.Core;
using NUnit.Framework;
using System.Collections.Generic;

namespace BDDfy.Tests.Arguments
{
    public class ArgumentsProvidedForGiven
    {
        private List<int> _andGivenInput1 = new List<int>();
        private List<int> _andGivenInput2 = new List<int>();
        private List<int> _andGivenInput3 = new List<int>();

        private int _givenInput3;
        private int _givenInput2;
        private int _givenInput1;

        [RunStepWithArgs(1, 2, 3)]
        void GivenOneSetOfArgumentsAreProvidedForGivenPart(int input1, int input2, int input3)
        {
            _givenInput1 = input1;
            _givenInput2 = input2;
            _givenInput3 = input3;
        }

        [RunStepWithArgs(4, 5, 6)]
        [RunStepWithArgs(7, 8, 9)]
        void GivenMoreThanOneSetOfArgumentsAreProvidedForGivenPart(int input1, int input2, int input3)
        {
            _andGivenInput1.Add(input1);
            _andGivenInput2.Add(input2);
            _andGivenInput3.Add(input3);
        }

        void ThenOneSetOfArgumentsArePassedInProperly()
        {
            Assert.That(_givenInput1, Is.EqualTo(1));
            Assert.That(_givenInput2, Is.EqualTo(2));
            Assert.That(_givenInput3, Is.EqualTo(3));
        }

        void ThenSeveralSetsOfArgumentsArePassedInProperly()
        {
            Assert.That(_andGivenInput1.Count, Is.EqualTo(2));
            Assert.That(_andGivenInput1, Contains.Item(4));
            Assert.That(_andGivenInput1, Contains.Item(7));

            Assert.That(_andGivenInput2.Count, Is.EqualTo(2));
            Assert.That(_andGivenInput2, Contains.Item(5));
            Assert.That(_andGivenInput2, Contains.Item(8));

            Assert.That(_andGivenInput3.Count, Is.EqualTo(2));
            Assert.That(_andGivenInput3, Contains.Item(6));
            Assert.That(_andGivenInput3, Contains.Item(9));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}