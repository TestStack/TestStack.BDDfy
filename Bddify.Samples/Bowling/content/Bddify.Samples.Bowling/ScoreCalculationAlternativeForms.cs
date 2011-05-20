// This class represents a story.

using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.Bowling
{
	// You set a class as a story by decorating it with 'Story' attribute
	// Each story can have zero or more scenarios
	// You indicate story's scenarios using WithScenario attribute
    [Story(
        AsA = "As a player",
        IWant = "I want the system to calculate my total score",
        SoThat = "In order to know my performance")]
    public class ScoreCalculationAlternativeForms
    {
        [Test]
        public void Execute()
        {
            typeof(ScoreCalculationAlternativeForms).Bddify();
        }
    }
}