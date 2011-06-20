using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class VerticalWinInTheRight : GameUnderTest
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
            Assert.AreEqual(Game.Winner, X);
        }
    }
}