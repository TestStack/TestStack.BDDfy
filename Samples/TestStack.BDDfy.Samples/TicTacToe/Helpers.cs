using Shouldly;

namespace TestStack.BDDfy.Samples.TicTacToe
{
    public static class Helpers
    {
        public static void VerifyBoardState(this Game game, string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            game.Equals(new Game(firstRow, secondRow, thirdRow)).ShouldBe(true);
        }
    }

    public class GameUnderTest
    {
        protected const string BoardStateTemplate = "Given the board\r\n{0}\r\n{1}\r\n{2}";

        protected const string X = Game.X;
        protected const string O = Game.O;
        protected const string N = Game.N;

        protected Game Game { get; set; }
    }

    public abstract class NewGame : GameUnderTest
    {
        protected void GivenANewGame()
        {
            Game = new Game();
        }
    }
}