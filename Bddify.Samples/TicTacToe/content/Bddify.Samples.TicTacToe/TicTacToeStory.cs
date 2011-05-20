using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    public class TicTacToeStory
    {
        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            typeof (WhenXAndOPlayTheirFirstMoves).Bddify();
        }

        [Test]
        public void HorizontalWin()
        {
            typeof (HorizontalWin).Bddify();
        }

        [Test]
        public void HorizontalWinInTheBottom()
        {
            typeof (HorizontalWinInTheBottom).Bddify();
        }

        [Test]
        public void HorizontalWinInTheMiddle()
        {
            typeof (HorizontalWinInTheMiddle).Bddify();
        }

        [Test]
        public void VerticalWinInTheLeft()
        {
            typeof (VerticalWinInTheLeft).Bddify();
        }

        [Test]
        public void VerticalWinInTheMiddle()
        {
            typeof (VerticalWinInTheMiddle).Bddify();
        }

        [Test]
        public void VerticalWinInTheRight()
        {
            typeof (VerticalWinInTheRight).Bddify();
        }

        [Test]
        public void OWins()
        {
            typeof (OWins).Bddify();
        }

        [Test]
        public void XWins()
        {
            typeof (XWins).Bddify();
        }

        [Test]
        public void CatsGame()
        {
            typeof (CatsGame).Bddify();
        }

        [Test]
        public void DiagonalWin()
        {
            typeof (DiagonalWin).Bddify();
        }
    }
}