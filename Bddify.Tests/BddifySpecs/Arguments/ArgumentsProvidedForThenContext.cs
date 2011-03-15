using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Arguments
{
    public class ArgumentsProvidedForThenContext
    {
        void GivenArgumentsAreProvidedForThenPart()
        {
        }

        [WithArgs(1, "Test")]
        void ThenArgumentsAreSentToThenPart(int argument1, string argument2)
        {
            Assert.That(argument1, Is.EqualTo(1));
            Assert.That(argument2, Is.EqualTo("Test"));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}