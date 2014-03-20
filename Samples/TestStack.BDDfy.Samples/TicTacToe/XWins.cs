using NUnit.Framework;
namespace TestStack.BDDfy.Samples.TicTacToe
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
            Assert.AreEqual(X, Game.Winner);
        }
    }
}