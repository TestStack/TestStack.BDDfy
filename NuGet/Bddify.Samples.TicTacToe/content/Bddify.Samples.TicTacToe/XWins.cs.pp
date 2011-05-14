using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class XWins : GameUnderTest
    {
        [RunStepWithArgs(
                new[] { X, X, O },
                new[] { X, X, O },
                new[] { O, O, N },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void WhenXPlaysInTheBottomRight()
        {
            Game.PlayAt(2, 2);
        }

        void ThenTheWinnerShouldBeX()
        {
            Assert.AreEqual(Game.Winner, X);
        }
    }
}