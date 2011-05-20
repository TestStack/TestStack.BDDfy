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
