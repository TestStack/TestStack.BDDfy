using Bddify.Core;
using NUnit.Framework;

namespace Demos.NUnit.CalculatorStory
{
    [Story(
        AsA = "As a second grader",
        IWant = "I want a calculator with four main functions",
        SoThat = "So I do not have to learn to calculate!!")]
    public class CalculatorCanDoFourMainMathFunctions
    {
        [Test]
        public void Execute()
        {
            BddifyExtensions.Bddify(this);
        }
    }
}