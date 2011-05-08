using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class VerticalWinInTheLeft : GameInProgress
    {
        [RunStepWithArgs(
                new[] { X, O, O },
                new[] { X, O, X },
                new[] { X, X, O },
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