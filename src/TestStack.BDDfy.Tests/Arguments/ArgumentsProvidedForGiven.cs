using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Arguments
{
    public class ArgumentsProvidedForGiven
    {
        private readonly List<int> _andGivenInput1 = new();
        private List<int> _andGivenInput2 = new();
        private List<int> _andGivenInput3 = new();

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
            _givenInput1.ShouldBe(1);
            _givenInput2.ShouldBe(2);
            _givenInput3.ShouldBe(3);
        }

        void ThenSeveralSetsOfArgumentsArePassedInProperly()
        {
            _andGivenInput1.Count.ShouldBe(2);
            _andGivenInput1.ShouldContain(4);
            _andGivenInput1.ShouldContain(7);

            _andGivenInput2.Count.ShouldBe(2);
            _andGivenInput2.ShouldContain(5);
            _andGivenInput2.ShouldContain(8);

            _andGivenInput3.Count.ShouldBe(2);
            _andGivenInput3.ShouldContain(6);
            _andGivenInput3.ShouldContain(9);
        }

        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}