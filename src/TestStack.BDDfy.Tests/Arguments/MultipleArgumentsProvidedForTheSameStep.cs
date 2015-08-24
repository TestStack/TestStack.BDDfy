using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Arguments
{
    public class MultipleArgumentsProvidedForTheSameStep
    {
        private readonly List<int> _inputs = new List<int>();

        [RunStepWithArgs(1)]
        [RunStepWithArgs(2)]
        [RunStepWithArgs(3)]
        void GivenMultipleArgumentAttributesAreProvidedToSameMethod(int input)
        {
            _inputs.Add(input);
        }

        void ThenTheMethodIsCalledOncePerArgument()
        {
            _inputs.Count.ShouldBe(3);
            _inputs.ShouldContain(1);
            _inputs.ShouldContain(2);
            _inputs.ShouldContain(3);
        }

        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}