using Shouldly;
using Xunit;
using Xunit.Extensions;

namespace TestStack.BDDfy.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    public class TicTacToe : NewGame
    {
        class Cell(int row, int col)
        {
            public int Row { get; set; } = row;
            public int Col { get; set; } = col;

            public override string ToString()
            {
                return string.Format("({0}, {1})", Row, Col);
            }
        }

        void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdrow)
        {
            Game = new Game(firstRow, secondRow, thirdrow);
        }

        void ThenTheBoardStateShouldBe(string[] firstRow, string[] secondRow, string[] thirdrow)
        {
            Game.VerifyBoardState(firstRow, secondRow, thirdrow);
        }

        void ThenTheWinnerShouldBe(string expectedWinner)
        {
            Game.Winner.ShouldBe(expectedWinner);
        }

        void ThenItShouldBeACatsGame()
        {
            Game.Winner.ShouldBe(null);
        }

        void WhenTheGameIsPlayedAt(params Cell[] cells)
        {
            foreach (var cell in cells)
            {
                Game.PlayAt(cell.Row, cell.Col);
            }
        }

        [Fact]
        public void CatsGame()
        {
            this.Given(s => s.GivenTheFollowingBoard(new[] { X, O, X }, new[] { O, O, X }, new[] { X, X, O }), BoardStateTemplate)
                .Then(s => s.ThenItShouldBeACatsGame())
                .BDDfy("Cat's game");
        }

        [Fact]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            this.Given(s => s.GivenANewGame())
                .When(s => s.WhenTheGameIsPlayedAt(new Cell(0, 0), new Cell(2, 2)), "When X and O play on {0}")
                .Then(s => s.ThenTheBoardStateShouldBe(new[] { X, N, N }, new[] { N, N, N }, new[] { N, N, O }))
                .BDDfy();
        }

        [Fact]
        public void OWins()
        {
            var cell = new Cell(2, 0);
            this.Given(s => s.GivenTheFollowingBoard(new[] { X, X, O }, new[] { X, O, N }, new[] { N, N, N }))
                .When(s => s.WhenTheGameIsPlayedAt(cell))
                .Then(s => s.ThenTheWinnerShouldBe(O))
                .BDDfy();
        }

        [Fact]
        public void XWins()
        {
            // This is implemented like this to show you the possibilities
            new XWins().BDDfy();
        }

        [Theory]
        [InlineData("Vertical win in the right", new[] { X, O, X }, new[] { O, O, X }, new[] { O, X, X }, X)]
        [InlineData("Vertical win in the middle", new[] { N, X, O }, new[] { O, X, O }, new[] { O, X, X }, X)]
        [InlineData("Diagonal win", new[] { X, O, O }, new[] { X, O, X }, new[] { O, X, N }, O)]
        [InlineData("Horizontal win in the bottom", new[] { X, X, N }, new[] { X, O, X }, new[] { O, O, O }, O)]
        [InlineData("Horizontal win in the middle", new[] { X, O, O }, new[] { X, X, X }, new[] { O, O, X }, X)]
        [InlineData("Vertical win in the left", new[] { X, O, O }, new[] { X, O, X }, new[] { X, X, O }, X)]
        [InlineData("Horizontal win", new[] { X, X, X }, new[] { X, O, O }, new[] { O, O, X }, X)]
        public void WinnerGame(string title, string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
        {
            new WinnerGame(firstRow, secondRow, thirdRow, expectedWinner).BDDfy<TicTacToe>(title);
        }
    }
}
