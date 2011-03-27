using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs.Arguments
{
    public class WhenArgumentsAreProvidedForGiven
    {
        private int _input3;
        private int _input2;
        private int _input1;

        [RunStepWithArgs(1, 2, 3)]
        void GivenArgumentsAreProvidedForGivenPart(int input1, int input2, int input3)
        {
            _input1 = input1;
            _input2 = input2;
            _input3 = input3;
        }

        void ThenArgumentsArePassedInProperly()
        {
            Assert.That(_input1, Is.EqualTo(1));
            Assert.That(_input2, Is.EqualTo(2));
            Assert.That(_input3, Is.EqualTo(3));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}