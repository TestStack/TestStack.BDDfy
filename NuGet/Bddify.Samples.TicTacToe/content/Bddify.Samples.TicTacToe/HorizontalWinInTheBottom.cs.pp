using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public class HorizontalWinInTheBottom : GameUnderTest
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
            Assert.AreEqual(Game.Winner, O);
        }
    }
}