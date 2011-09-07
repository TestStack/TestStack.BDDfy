using Bddify.Core;
using NUnit.Framework;


namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    [TestFixture]
    public class TicTacToe : NewGame
    {
        class Cell
        {
            public Cell(int row, int col)
            {
                Row = row;
                Col = col;
            }

            public int Row { get; set; }
            public int Col { get; set; }

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
            Assert.AreEqual(expectedWinner, Game.Winner);
        }

        void ThenItShouldBeACatsGame()
        {
            Assert.IsNull(Game.Winner);
        }

        void WhenTheGameIsPlayedAt(params Cell[] cells)
        {
            foreach (var cell in cells)
            {
                Game.PlayAt(cell.Row, cell.Col);
            }
        }

        [Test]
        public void CatsGame()
        {
            this.Given(s => s.GivenTheFollowingBoard(new[] { X, O, X }, new[] { O, O, X }, new[] { X, X, O }), BoardStateTemplate)
                .Then(s => s.ThenItShouldBeACatsGame())
                .Bddify("Cat's game");
        }

        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            this.Given(s => s.GivenANewGame())
                .When(s => s.WhenTheGameIsPlayedAt(new Cell(0, 0), new Cell(2, 2)), "When X and O play on {0}")
                .Then(s => s.ThenTheBoardStateShouldBe(new[] { X, N, N }, new[] { N, N, N }, new[] { N, N, O }))
                .Bddify();
        }

        [Test]
        public void OWins()
        {
            var cell = new Cell(2, 0);
            this.Given(s => s.GivenTheFollowingBoard(new[] { X, X, O }, new[] { X, O, N }, new[] { N, N, N }))
                .When(s => s.WhenTheGameIsPlayedAt(cell))
                .Then(s => s.ThenTheWinnerShouldBe(O))
                .Bddify();
        }

        [Test]
        public void XWins()
        {
            // This is implemented like this to show you the possibilities
            new XWins().Bddify();
        }

        [Test]
        [TestCase("Vertical win in the right", new[] { X, O, X }, new[] { O, O, X }, new[] { O, X, X }, X)]
        [TestCase("Vertical win in the middle", new[] { N, X, O }, new[] { O, X, O }, new[] { O, X, X }, X)]
        [TestCase("Diagonal win", new[] { X, O, O }, new[] { X, O, X }, new[] { O, X, N }, O)]
        [TestCase("Horizontal win in the bottom", new[] { X, X, N }, new[] { X, O, X }, new[] { O, O, O }, O)]
        [TestCase("Horizontal win in the middle", new[] { X, O, O }, new[] { X, X, X }, new[] { O, O, X }, X)]
        [TestCase("Vertical win in the left", new[] { X, O, O }, new[] { X, O, X }, new[] { X, X, O }, X)]
        [TestCase("Horizontal win", new[] { X, X, X }, new[] { X, O, O }, new[] { O, O, X }, X)]
        public void WinnerGame(string title, string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
        {
            new WinnerGame(firstRow, secondRow, thirdRow, expectedWinner).Bddify<TicTacToe>(title);
        }
    }
}
