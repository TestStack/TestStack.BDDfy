using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class DiagonalWin : GameUnderTest
    {
        [RunStepWithArgs(
                new[] { X, O, O },
                new[] { X, O, X },
                new[] { O, X, N },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void ThenTheWinnerShouldBeO()
        {
            Assert.AreEqual(Game.Winner, O);
        }
    }
}