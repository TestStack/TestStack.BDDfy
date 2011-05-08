using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class VerticalWinInTheRight : GameInProgress
    {
        [RunStepWithArgs(
                new[] { X, O, X },
                new[] { O, O, X },
                new[] { O, X, X },
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