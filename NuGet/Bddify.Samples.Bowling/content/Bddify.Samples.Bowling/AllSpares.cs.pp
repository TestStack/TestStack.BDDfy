using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Bowling
{
    public class AllSpares
    {
        private Game _game;

        void GivenANewBowlingGame()
        {
            _game = new Game();
        }

        [RunStepWithArgs(10, 1, 9, StepTextTemplate = "When I roll {0} times {1} and {2}")]
        void WhenIRollSeveralTimes(int rollCount, int pins1, int pins2)
        {
            for (int i = 0; i < rollCount; i++)
            {
                _game.Roll(pins1);
                _game.Roll(pins2);
            }
        }

        void AndWhenIRoll1()
        {
            _game.Roll(1);
        }

        void ThenMyTotalScoreShouldBe110()
        {
            Assert.That(_game.Score, Is.EqualTo(110));
        }
    }
}