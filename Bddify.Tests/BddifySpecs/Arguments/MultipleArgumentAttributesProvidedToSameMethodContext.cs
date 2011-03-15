using NUnit.Framework;
using System.Collections.Generic;

namespace Bddify.Tests.BddifySpecs.Arguments
{
    public class MultipleArgumentAttributesProvidedToSameMethodContext
    {
        private readonly List<int> _inputs = new List<int>();

        [WithArgs(1)]
        [WithArgs(2)]
        [WithArgs(3)]
        void GivenMultipleArgumentAttributesAreProvidedToSameMethod(int input)
        {
            _inputs.Add(input);
        }

        void ThenMethodIsCalledOncePerArgument()
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