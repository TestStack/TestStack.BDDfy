using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class OWins : GameInProgress
    {
        [RunStepWithArgs(
                new[] { X, X, O },
                new[] { X, O, N },
                new[] { N, N, N },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void WhenOPlaysInTheBottomLeft()
        {
            Game.PlayAt(2, 0);
        }

        void ThenTheWinnerShouldBeO()
        {
            Assert.That(Game.Winner, Is.EqualTo(O));
        }
    }
}