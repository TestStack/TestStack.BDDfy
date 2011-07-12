using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    [Story(
        AsA = "As a player",
        IWant = "I want to have a tic tac toe game",
        SoThat = "So that I can waste some time!")]
    [TestFixture]
    public class TicTacToeStory
    {
        private const string X = Game.X;
        private const string O = Game.O;
        private const string N = Game.N;

        [Test]
        public void WhenXAndOPlayTheirFirstMoves()
        {
            new WhenXAndOPlayTheirFirstMoves().Bddify();
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
        [TestCase("Vertical win in the right", new[] { X, O, X }, new[] { O, O, X }, new[] { O, X, X }, X)]
        [TestCase("Vertical win in the middle", new[] { N, X, O }, new[] { O, X, O }, new[] { O, X, X }, X)]
        [TestCase("Diagonal win", new[] { X, O, O }, new[] { X, O, X }, new[] { O, X, N }, O)]
        [TestCase("Horizontal win in the bottom", new[] { X, X, N }, new[] { X, O, X }, new[] { O, O, O }, O)]
        [TestCase("Horizontal win in the middle", new[] { X, O, O }, new[] { X, X, X }, new[] { O, O, X }, X)]
        [TestCase("Vertical win in the left", new[] { X, O, O }, new[] { X, O, X }, new[] { X, X, O }, X)]
        [TestCase("Horizontal win", new[] { X, X, X }, new[] { X, O, O }, new[] { O, O, X }, X)]
        public void WinnerGame(string title, string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
        {
            new WinnerGame(firstRow, secondRow, thirdRow, expectedWinner).Bddify(scenarioTitle: title);
        }
    }
}