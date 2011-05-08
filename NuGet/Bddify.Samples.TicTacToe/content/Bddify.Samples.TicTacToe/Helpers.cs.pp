using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.TicTacToe
{
    public static class Helpers
    {
        public static void VerifyBoardState(this Game game, string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Assert.True(game.Equals(new Game(firstRow, secondRow, thirdRow)));
        }
    }

    public class GameUnderTest
    {
        protected const string X = Game.X;
        protected const string O = Game.O;
        protected const string N = Game.N;

        protected Game Game { get; set; }
    }

    public abstract class GameInProgress : GameUnderTest
    {
        protected const string BoardStateTemplate = "Given the board rows looks like [{0}], [{1}] and [{2}]";
    }

    public abstract class NewGame : GameUnderTest
    {
        protected void GivenANewGame()
        {
            Game = new Game();
        }
    }
}