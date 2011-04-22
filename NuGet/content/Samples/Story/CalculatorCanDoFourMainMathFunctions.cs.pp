using System;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$.Samples.Story
{
	[Story(
    AsA = "As a second grader",
    IWant = "I want a calculator with four main functions",
    SoThat = "So I do not have to learn to calculate!!")]
	[WithScenario(typeof(WhenTwoNumbersAreAdded))]
	[WithScenario(typeof(WhenTwoNumbersAreDevided))]
	[WithScenario(typeof(WhenTwoNumbersAreMultiplied))]
	[WithScenario(typeof(WhenTwoNumbersAreSubtracted))]
    [TestClass]
	public class CalculatorCanDoFourMainMathFunctions
    {
        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }
}