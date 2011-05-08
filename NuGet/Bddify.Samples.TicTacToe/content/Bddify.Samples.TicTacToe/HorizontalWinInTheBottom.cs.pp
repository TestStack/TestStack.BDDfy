using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class HorizontalWinInTheBottom : GameInProgress
    {
        [RunStepWithArgs(
                new[] { X, X, N },
                new[] { X, O, X },
                new[] { O, O, O },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void ThenTheWinnerShouldBeO()
        {
            Assert.That(Game.Winner, Is.EqualTo(O));
        }
    }
}