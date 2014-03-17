using NUnit.Framework;

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
            Assert.That(argument1, Is.EqualTo(1));
            Assert.That(argument2, Is.EqualTo("Test"));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}