using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class HorizontalWin : GameInProgress
    {
        [RunStepWithArgs(
                new[] { X, X, X },
                new[] { X, O, O },
                new[] { O, O, X },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void ThenTheWinnerShouldBeX()
        {
            Assert.That(Game.Winner, Is.EqualTo(X));
        }
    }
}