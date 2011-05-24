using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;


namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
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
            Assert.AreEqual(Game.Winner, expectedWinner);
        }

        public void ThenItShouldBeACatsGame()
        {
            Assert.AreEqual(Game.Winner, null);
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
            FluentStepScanner<TicTacToeStoryWithFluentScanner>
                .Scan()
                    .Given(s => s.GivenTheFollowingBoard(new[] { X, O, X }, new[] { O, O, X }, new[] { X, X, O }), BoardStateTemplate)
                    .Then(s => s.ThenItShouldBeACatsGame())
                .Bddify("Cat's game");
        }

        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            var firstMove = new Cell(0, 0);
            var secondMove = new Cell(2, 2);
            FluentStepScanner<TicTacToeStoryWithFluentScanner>
                .Scan()
                .Given(s => s.GivenANewGame())
                .When(s => s.WhenTheGameIsPlayedAt(firstMove, secondMove), "When X and O play on {0}")
                .Then(s => s.ThenTheBoardStateShouldBe(new[] { X, N, N }, new[] { N, N, N }, new[] { N, N, O }))
                .Bddify();
        }

        [Test]
        public void HorizontalWin()
        {
            AssertWinningScenario(
                new[] { X, X, X },
                new[] { X, O, O },
                new[] { O, O, X },
                X);
        }

        [Test]
        public void HorizontalWinInTheBottom()
        {
            AssertWinningScenario(
                new[] { X, X, N },
                new[] { X, O, X },
                new[] { O, O, O },
                O);
        }

        [Test]
        public void HorizontalWinInTheMiddle()
        {
            AssertWinningScenario(
                new[] { X, O, O },
                new[] { X, X, X },
                new[] { O, O, X },
                X);
        }

        [Test]
        public void VerticalWinInTheLeft()
        {
            AssertWinningScenario(
                new[] { X, O, O },
                new[] { X, O, X },
                new[] { X, X, O },
                X);
        }

        [Test]
        public void VerticalWinInTheMiddle()
        {
            AssertWinningScenario(
                new[] { X, X, O },
                new[] { O, X, O },
                new[] { O, X, X },
                X);
        }

        [Test]
        public void VerticalWinInTheRight()
        {
            AssertWinningScenario(
                new[] { X, O, X },
                new[] { O, O, X },
                new[] { O, X, X },
                X);
        }

        [Test]
        public void OWins()
        {
            var cell = new Cell(2, 0);
            FluentStepScanner<TicTacToeStoryWithFluentScanner>
                .Scan()
                .Given(s => s.GivenTheFollowingBoard(new[] { X, X, O }, new[] { X, O, N }, new[] { N, N, N }))
                .When(s => s.WhenTheGameIsPlayedAt(cell))
                .Then(s => s.ThenTheWinnerShouldBe(O))
                .Bddify();
        }

        [Test]
        public void XWins()
        {
            var cell = new Cell(2, 2);
            FluentStepScanner<TicTacToeStoryWithFluentScanner>
                .Scan()
                .Given(s => s.GivenTheFollowingBoard(new[] { X, X, O }, new[] { X, X, O }, new[] { O, O, N }))
                .When(s => s.WhenTheGameIsPlayedAt(cell))
                .Then(s => s.ThenTheWinnerShouldBe(X))
                .Bddify();
        }

        [Test]
        public void DiagonalWin()
        {
            AssertWinningScenario(
                new[] { X, O, O },
                new[] { X, O, X },
                new[] { O, X, N },
                O);
        }

        void AssertWinningScenario(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
        {
            FluentStepScanner<TicTacToeStoryWithFluentScanner>
                .Scan()
                .Given(s => s.GivenTheFollowingBoard(firstRow, secondRow, thirdRow), BoardStateTemplate)
                .Then(s => ThenTheWinnerShouldBe(expectedWinner))
                .Bddify();
        }
    }
}
