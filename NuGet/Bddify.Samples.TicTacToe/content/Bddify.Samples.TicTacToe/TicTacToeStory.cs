using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    [WithScenario(typeof(WhenXAndOPlayTheirFirstMoves))]
    [WithScenario(typeof(HorizontalWin))]
    [WithScenario(typeof(HorizontalWinInTheBottom))]
    [WithScenario(typeof(HorizontalWinInTheMiddle))]
    [WithScenario(typeof(VerticalWinInTheLeft))]
    [WithScenario(typeof(VerticalWinInTheMiddle))]
    [WithScenario(typeof(VerticalWinInTheRight))]
    [WithScenario(typeof(OWins))]
    [WithScenario(typeof(XWins))]
    [WithScenario(typeof(CatsGame))]
    [WithScenario(typeof(DiagonalWin))]
    public class TicTacToeStory
    {
        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            new WhenXAndOPlayTheirFirstMoves().Bddify();
        }

        [Test]
        public void HorizontalWin()
        {
            new HorizontalWin().Bddify();
        }

        [Test]
        public void HorizontalWinInTheBottom()
        {
            new HorizontalWinInTheBottom().Bddify();
        }

        [Test]
        public void HorizontalWinInTheMiddle()
        {
            new HorizontalWinInTheMiddle().Bddify();
        }
 
        [Test]
        public void VerticalWinInTheLeft()
        {
            new VerticalWinInTheLeft().Bddify();
        }

        [Test]
        public void VerticalWinInTheMiddle()
        {
            new VerticalWinInTheMiddle().Bddify();
        }

        [Test]
        public void VerticalWinInTheRight()
        {
            new VerticalWinInTheRight().Bddify();
        }

        [Test]
        public void OWins()
        {
            new OWins().Bddify();
        }

        [Test]
        public void XWins()
        {
            new XWins().Bddify();
        }

        [Test]
        public void CatsGame()
        {
            new CatsGame().Bddify();
        }

        [Test]
        public void DiagonalWin()
        {
            new DiagonalWin().Bddify();
        }
    }
}