using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Collections.Generic;

namespace Bddify.Tests.BddifySpecs.Arguments
{
    public class WhenMultipleArgumentsAreProvidedForTheSameStep
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
            Assert.That(_inputs.Count, Is.EqualTo(3));
            Assert.That(_inputs, Contains.Item(1));
            Assert.That(_inputs, Contains.Item(2));
            Assert.That(_inputs, Contains.Item(3));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}