using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class OWins : GameUnderTest
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
            Assert.AreEqual(O, Game.Winner);
        }
    }
}