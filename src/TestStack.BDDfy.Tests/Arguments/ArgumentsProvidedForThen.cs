using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Arguments
{
    public class ArgumentsProvidedForThen
    {
        void GivenArgumentsAreProvidedForThenPart()
        {
        }

        [RunStepWithArgs(1, "Test")]
        void ThenArgumentsAreSentToThenPart(int argument1, string argument2)
        {
            argument1.ShouldBe(1);
            argument2.ShouldBe("Test");
        }

        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}