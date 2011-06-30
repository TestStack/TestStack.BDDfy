using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class CatsGame : GameUnderTest
    {
        [RunStepWithArgs(
                new[] { X, O, X },
                new[] { O, O, X },
                new[] { X, X, O },
                StepTextTemplate = BoardStateTemplate)]
        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game = new Game(firstRow, secondRow, thirdRow);
        }

        void ThenItShouldBeACatsGame()
        {
            Assert.IsNull(Game.Winner);
        }
    }
}