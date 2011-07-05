using Bddify.Core;
using NUnit.Framework;


namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    [TestFixture]
    public class TicTacToeStoryWithFluentScanner : NewGame
    {
        public class Cell
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

        public void GivenTheFollowingBoard(string[] firstRow, string[] secondRow, string[] thirdrow)
        {
            Game = new Game(firstRow, secondRow, thirdrow);
        }

        public void ThenTheBoardStateShouldBe(string[] firstRow, string[] secondRow, string[] thirdrow)
        {
            Game.VerifyBoardState(firstRow, secondRow, thirdrow);
        }

        public void ThenTheWinnerShouldBe(string expectedWinner)
        {
            Assert.AreEqual(expectedWinner, Game.Winner);
        }

        public void ThenItShouldBeACatsGame()
        {
            Assert.IsNull(Game.Winner);
        }

        public void WhenTheGameIsPlayedAt(params Cell[] cells)
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
            this.Given(s => s.GivenTheFollowingBoard(new[] { X, X, O }, new[] { X, X, O }, new[] { O, O, N }))
                .When(s => s.WhenTheGameIsPlayedAt(new Cell(2, 2)))
                .Then(s => s.ThenTheWinnerShouldBe(X))
                .Bddify();
        }

        [Test]
        [TestCase(
            new[] { X, O, O }, 
            new[] { X, O, X }, 
            new[] { O, X, N }, 
            O, "Diagonal win")]
        [TestCase(
            new[] { X, O, X }, 
            new[] { O, O, X }, 
            new[] { O, X, X }, 
            X, "Vertical win in the right")]
        [TestCase(
            new[] { X, X, O }, 
            new[] { O, X, O }, 
            new[] { O, X, X }, 
            X, "Vertical in in the middle")]
        [TestCase(
            new[] { X, O, O },
            new[] { X, O, X },
            new[] { X, X, O },
            X, "Vertical in in the left")]
        [TestCase(
            new[] { X, O, O },
            new[] { X, X, X },
            new[] { O, O, X },
            X, "Horizontal win in the middle")]
        [TestCase(
            new[] { X, X, N },
            new[] { X, O, X },
            new[] { O, O, O },
            O, "Horizontal in in the bottom")]
        [TestCase(
            new[] { X, X, X },
            new[] { X, O, O },
            new[] { O, O, X },
            X, "Horizontal win")]
        public void WinningScenario(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner, string scenarioTitle)
        {
            this.Given(s => s.GivenTheFollowingBoard(firstRow, secondRow, thirdRow), BoardStateTemplate)
                .Then(s => ThenTheWinnerShouldBe(expectedWinner))
                .Bddify(scenarioTitle);
        }
    }
}
