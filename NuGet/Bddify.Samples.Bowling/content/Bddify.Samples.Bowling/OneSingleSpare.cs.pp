// This class serves as a scenario for ScoreCalculationAlternativeForms story.

using System.Collections.Generic;
using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Bowling
{
    public class OneSingleSpare
    {
        private Game _game;

        void GivenANewBowlingGame()
        {
            _game = new Game();
        }

        [RunStepWithArgs(new[] {3, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1})]
        void WhenIRollTheFollowingSeries(IEnumerable<int> pinsSeries)
        {
            foreach (var pins in pinsSeries)
            {
                _game.Roll(pins);
            }
        }

        void ThenMyTotalScoreShouldBe29()
        {
            Assert.That(_game.Score, Is.EqualTo(29));
        }
    }
}