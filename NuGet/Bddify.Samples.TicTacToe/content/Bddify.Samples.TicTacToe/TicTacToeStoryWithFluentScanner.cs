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

        public void WhenTheGameIsPlayedAtTheFollowingRowAndColumn(int row, int column)
        {
            Game.PlayAt(row, column);
        }

        static FluentStepScanner<TicTacToeStoryWithFluentScanner> Scanner
        {
            get
            {
                return new FluentStepScanner<TicTacToeStoryWithFluentScanner>();
            }
        }

        [Test]
        public void CatsGame()
        {
            var scanner = 
                Scanner
                    .Given(s => s.GivenTheFollowingBoard(new[] { X, O, X }, new[] { O, O, X }, new[] { X, X, O }), BoardStateTemplate)
                    .Then(s => s.ThenItShouldBeACatsGame());
            this.Bddify(scanner, scenarioTextTemplate: "Cat's game");
        }

        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            
        }

        [Test]
        public void HorizontalWin()
        {

        }

        public void HorizontalWinInTheBottom()
        { }

        [Test]
        public void HorizontalWinInTheMiddle() { }
 
        [Test]
        public void VerticalWinInTheLeft() { }

        [Test]
        public void VerticalWinInTheMiddle() { }
        
        [Test]
        public void VerticalWinInTheRight() { }

        [Test]
        public void OWins() { }
        
        [Test]
        public void XWins() { }
        
        [Test]
        public void DiagonalWin()
        {
            AssertWinningScenario(
                new[] { X, O, O }, 
                new[] { X, O, X }, 
                new[] { O, X, N }, 
                O, 
                "Diagonal Win");
        }

        void AssertWinningScenario(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner, string scenarioTitle)
        {
            var scanner =
                Scanner
                    .Given(s => s.GivenTheFollowingBoard(firstRow, secondRow, thirdRow), BoardStateTemplate)
                    .Then(s => ThenTheWinnerShouldBe(expectedWinner));
            this.Bddify(scanner, scenarioTextTemplate: scenarioTitle);
        }
    }
}        
