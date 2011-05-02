// This class represents a story.

using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Bowling
{
	// You set a class as a story by decorating it with 'Story' attribute
	// Each story can have one or more scenarios (though bddify will not crash if you do not provide any scenarios either)
	// You indicate story's scenarios using WithScenario attribute
    [Story(
        AsA = "As a player",
        IWant = "I want the system to calculate my total score",
        SoThat = "In order to know my performance")]
	[WithScenario(typeof(OneSingleSpare))]
    [WithScenario(typeof(AllSpares))]
    public class ScoreCalculationAlternativeForms
    {
        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}