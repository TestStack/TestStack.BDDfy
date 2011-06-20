using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class VerticalWinInTheLeft : GameUnderTest
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
            Assert.AreEqual(Game.Winner, X);
        }
    }
}