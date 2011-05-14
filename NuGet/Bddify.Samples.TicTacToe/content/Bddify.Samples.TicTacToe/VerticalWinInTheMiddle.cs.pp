using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class VerticalWinInTheMiddle : GameUnderTest
    {
        [RunStepWithArgs(
                new[] { X, X, O },
                new[] { O, X, O },
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