using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Bowling
{
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