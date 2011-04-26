using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.NUnit.CalculatorStory
{
    [Story(
        AsA = "As a second grader",
        IWant = "I want a calculator with four main functions",
        SoThat = "So I do not have to learn to calculate!!")]
    [WithScenario(typeof(WhenTwoNumbersAreAdded))]
    [WithScenario(typeof(WhenTwoNumbersAreDevided))]
    [WithScenario(typeof(WhenTwoNumbersAreMultiplied))]
    [WithScenario(typeof(WhenTwoNumbersAreSubtracted))]
    public class CalculatorCanDoFourMainMathFunctions
    {
        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}